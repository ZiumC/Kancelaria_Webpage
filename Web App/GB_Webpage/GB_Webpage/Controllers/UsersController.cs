using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.Database.DatabaseFiles;
using GB_Webpage.Services.Database.Users;
using GB_Webpage.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IDatabaseFileService _databaseFileService;
        private readonly ILogger<UsersController> _logger;
        private readonly IApiUsersService _usersService;
        private readonly IUserService _userService;

        private readonly string _refreshTokenFolder;
        //private readonly string _issuer;
        //private readonly string _secretSignature;
        //private readonly int _daysValid;
        private readonly int _maxAttemps;
        private readonly int _suspendDurationDays;


        private readonly Dictionary<int, string> _statuses;
        private readonly int OK = 200, UNAUTHORIZED = 403, NOT_FOUND = 404, TOKEN_BROKEN = 452;

        public UsersController(IConfiguration configuration, IDatabaseFileService databaseFileService, ILogger<UsersController> logger, IApiUsersService apiUsersService, IUserService userService)
        {
            _databaseFileService = databaseFileService;
            _usersService = apiUsersService;
            _configuration = configuration;
            _userService = userService;
            _logger = logger;

            //_issuer = _configuration["profiles:GB_Webpage:applicationUrl"].Split(";")[0];
            //_secretSignature = _configuration["SecretSignatureKey"];
            //_daysValid = int.Parse(_configuration["profiles:GB_Webpage:DaysValidToken"]);
            _refreshTokenFolder = _configuration["Paths:DatabaseStorage:RefreshTokenFolder"];
            //_maxAttemps = int.Parse(_configuration["MaxLoginAttemps"]);

            int maxAttemps = 3;
            try
            {
                maxAttemps = int.Parse(_configuration["AppSettings:MaxLoginAttemps"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
            }
            _maxAttemps = maxAttemps;

            int suspendDurationDays = 1;
            try
            {
                suspendDurationDays = int.Parse(_configuration["AppSettings:BlockadeDaysDuration"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
            }
            _suspendDurationDays = suspendDurationDays; 


            _statuses = new Dictionary<int, string>()
            {
                { 200, "login approved" },
                { 403, "unauthorized" },
                { 404, "data not found" },
                { 452, "tokens are broken" }
            };
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            string actionLog = "User logging in";
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));

            SuspendedUserModel? suspendedUserData = await _usersService.GetUserDataFromBlacklistAsync(request.Login);
            if (suspendedUserData != null)
            {
                if (suspendedUserData.Attemps >= _maxAttemps && suspendedUserData.SuspendedDateTo == null)
                {
                    suspendedUserData.SuspendedDateTo = DateTime.Now.AddMinutes(_suspendDurationDays);
                    suspendedUserData.Attemps -= 1;
                    bool isUpdated = await _usersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                            "Unable to update user in block list.",
                            LogFormatterService.GetMethodName())
                        );
                    }

                    var statusResponse = HttpService.SelectStatusBy(UNAUTHORIZED, _statuses);
                    _logger.LogWarning(LogFormatterService.FormatAction(
                        actionLog,
                        $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}). User is blocked to {suspendedUserData.SuspendedDateTo}.",
                        LogFormatterService.GetMethodName())
                    );

                    return Unauthorized($"You can't login due to {suspendedUserData.SuspendedDateTo}.");
                }

                if (suspendedUserData.SuspendedDateTo > DateTime.Now)
                {
                    return Unauthorized($"You can't login due to {suspendedUserData.SuspendedDateTo}.");
                }
                else
                {
                    suspendedUserData.SuspendedDateTo = null;
                    bool isUpdated = await _usersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                            "Unable to update user in block list.",
                            LogFormatterService.GetMethodName())
                        );
                    }
                }
            }

            if (!_userService.isUserValidFor(request))
            {
                var statusResponse = HttpService.SelectStatusBy(UNAUTHORIZED, _statuses);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                            $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}). User is invalid.",
                            LogFormatterService.GetMethodName())
                        );

                if (suspendedUserData == null)
                {
                    bool isUserAdded = await _usersService.AddUserToBlocklistAsync(request.Login);
                    if (!isUserAdded)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                           "Unable to add user to block list.",
                            LogFormatterService.GetMethodName())
                        );
                    }

                    return Unauthorized($"Login or password is wrong. You have {_maxAttemps} attemps left.");
                }
                else
                {
                    suspendedUserData.Attemps = suspendedUserData.Attemps + 1;
                    bool isUpdated = await _usersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                           actionLog,
                          "Unable to update user to block list.",
                           LogFormatterService.GetMethodName())
                       );
                    }
                    return Unauthorized($"Login or password is wrong. You have {_maxAttemps - suspendedUserData.Attemps} attemps left.");
                }
            }
            else
            {
                string refreshToken = _userService.GenerateRefreshToken();
                string accessToken = _userService.GenerateAccessToken(request.Login);

                _databaseFileService.SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = request.Login
                }, _refreshTokenFolder);

                var statusResponse = HttpService.SelectStatusBy(OK, _statuses);

                _logger.LogInformation(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}).",
                    LogFormatterService.GetMethodName())
                );

                if (suspendedUserData != null)
                {
                    suspendedUserData.Attemps = 0;
                    suspendedUserData.SuspendedDateTo = null;

                    bool isUpdated = await _usersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                           actionLog,
                          "Unable to update user to block list.",
                           LogFormatterService.GetMethodName())
                       );
                    }
                }

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult RefreshToken(JwtDTO jwt)
        {
            string actionLog = "Refreshing tokens";

            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));

            UserRefreshTokenModel? savedUserToken = _databaseFileService.ReadFile<UserRefreshTokenModel>(_refreshTokenFolder);
            if (savedUserToken == null)
            {
                var statusResponse = HttpService.SelectStatusBy(NOT_FOUND, _statuses);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetMethodName())
                );

                return NotFound("Refresh token not found");
            }

            if (!savedUserToken.RefreshToken.Equals(jwt.RefreshToken))
            {
                var statusResponse = HttpService.SelectStatusBy(TOKEN_BROKEN, _statuses);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetMethodName())
                );

                return StatusCode(452, "Tokens aren't valid to server,  login again");
            }

            bool areTokensValid = _userService.ValidateTokens(jwt);
            if (areTokensValid)
            {
                string refreshToken = _userService.GenerateRefreshToken();
                string accessToken = _userService.GenerateAccessToken(savedUserToken.UserName);

                _databaseFileService.SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = savedUserToken.UserName
                }, _refreshTokenFolder);

                var statusResponse = HttpService.SelectStatusBy(OK, _statuses);

                _logger.LogInformation(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetMethodName())
                );

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }
            else
            {
                var statusResponse = HttpService.SelectStatusBy(TOKEN_BROKEN, _statuses);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetMethodName())
                );

                return StatusCode(452, "Tokens aren't valid to this server, login again");
            }
        }
    }
}
