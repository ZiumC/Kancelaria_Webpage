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
        private readonly IApiUsersService _apiUsersService;
        private readonly IUserService _userService;

        private readonly string _suspendedUsersFolder;
        private readonly string _refreshTokenFolder;

        private readonly int _suspendDuration;
        private readonly int _maxAttemps;


        private readonly Dictionary<int, string> _statuses;
        private readonly int OK = 200, UNAUTHORIZED = 403, NOT_FOUND = 404, TOKEN_BROKEN = 452;

        public UsersController(IConfiguration configuration, IDatabaseFileService databaseFileService, ILogger<UsersController> logger, IApiUsersService apiUsersService, IUserService userService)
        {
            _databaseFileService = databaseFileService;
            _apiUsersService = apiUsersService;
            _configuration = configuration;
            _userService = userService;
            _logger = logger;

            _suspendedUsersFolder = _configuration["ApplicationSettings:DatabaseSettings:Paths:SuspendedUsersFolder"];
            _refreshTokenFolder = _configuration["ApplicationSettings:DatabaseSettings:Paths:RefreshTokenFolder"];

            _suspendDuration = ParseToInt(_configuration["ApplicationSettings:UsersSettings:SuspendedUsersSettings:SuspendDurationDays"], 1);
            _maxAttemps = ParseToInt(_configuration["ApplicationSettings:UsersSettings:SuspendedUsersSettings:MaxLoginAttemps"], 3);


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

            SuspendedUserModel? suspendedUserData = await _apiUsersService.GetUserDataFromBlacklistAsync(request.Login);
            if (suspendedUserData != null)
            {
                //Suspending user.
                if (suspendedUserData.Attempts >= _maxAttemps && suspendedUserData.SuspendedDateTo == null)
                {
                    suspendedUserData.SuspendedDateTo = DateTime.Now.AddDays(_suspendDuration);
                    //Decreasing attempt by one gives an user only one chance to correct login.
                    suspendedUserData.Attempts -= 1;
                    bool isUpdated = await _apiUsersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                            "Unable to update user in block list",
                            LogFormatterService.GetMethodName())
                        );
                    }

                    var statusResponse = HttpService.SelectStatusBy(UNAUTHORIZED, _statuses);
                    _logger.LogWarning(LogFormatterService.FormatAction(
                        actionLog,
                        $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}). User is suspended to {suspendedUserData.SuspendedDateTo}.",
                        LogFormatterService.GetMethodName())
                    );

                    SaveSuspendedUsers();

                    return Unauthorized($"You can't login due to {suspendedUserData.SuspendedDateTo}.");
                }

                //Checking if date is gt current date. If not suspend date is clearing.
                if (suspendedUserData.SuspendedDateTo > DateTime.Now)
                {
                    var statusResponse = HttpService.SelectStatusBy(UNAUTHORIZED, _statuses);
                    _logger.LogWarning(LogFormatterService.FormatAction(
                        actionLog,
                        $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}). User is suspended to {suspendedUserData.SuspendedDateTo}.",
                        LogFormatterService.GetMethodName())
                    );

                    return Unauthorized($"You can't login due to {suspendedUserData.SuspendedDateTo}.");
                }
                else
                {
                    suspendedUserData.SuspendedDateTo = null;
                    bool isUpdated = await _apiUsersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                            "Unable to clear suspended date",
                            LogFormatterService.GetMethodName())
                        );
                    }

                    _logger.LogInformation(LogFormatterService.FormatAction(
                        actionLog,
                        "Suspend date has been cleared",
                        LogFormatterService.GetMethodName())
                    );

                    SaveSuspendedUsers();
                }
            }

            //Access token will be returned if user is valid
            if (!_userService.isUserValidFor(request))
            {
                var statusResponse = HttpService.SelectStatusBy(UNAUTHORIZED, _statuses);
                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}). Username is invalid.",
                    LogFormatterService.GetMethodName())
                );

                //If user hadn't been suspended yet - it will be added to db. Otherwise attempts will be increased.
                if (suspendedUserData == null)
                {
                    bool isUserAdded = await _apiUsersService.AddUserToBlocklistAsync(request.Login);
                    if (!isUserAdded)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                            actionLog,
                            "Unable to add user to suspended users list.",
                            LogFormatterService.GetMethodName())
                        );
                    }

                    SaveSuspendedUsers();

                    return Unauthorized($"Login or password is wrong. You have {_maxAttemps} attemps left.");
                }
                else
                {
                    suspendedUserData.Attempts = suspendedUserData.Attempts + 1;
                    bool isUpdated = await _apiUsersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                           actionLog,
                          "Unable to update user in suspended users list",
                           LogFormatterService.GetMethodName())
                       );
                    }

                    SaveSuspendedUsers();

                    return Unauthorized($"Login or password is wrong. You have {_maxAttemps - suspendedUserData.Attempts} attemps left.");
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

                if (suspendedUserData != null)
                {
                    suspendedUserData.Attempts = 0;
                    suspendedUserData.SuspendedDateTo = null;

                    bool isUpdated = await _apiUsersService.UpdateUserInBlockedlistAsync(suspendedUserData.Id, suspendedUserData);
                    if (!isUpdated)
                    {
                        _logger.LogCritical(LogFormatterService.FormatAction(
                           actionLog,
                          "Unable to update user in suspended users list",
                           LogFormatterService.GetMethodName())
                       );
                    }

                    SaveSuspendedUsers();
                }

                var statusResponse = HttpService.SelectStatusBy(OK, _statuses);
                _logger.LogInformation(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (User={request.Login}).",
                    LogFormatterService.GetMethodName())
                );

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

        private async void SaveSuspendedUsers()
        {
            string actionLog = "Saving suspended user to db";
            IEnumerable<SuspendedUserModel> suspendedUser = await _apiUsersService.GetAllSuspendedUsers();
            if (suspendedUser != null)
            {
                _databaseFileService.SaveFile<IEnumerable<SuspendedUserModel>>(
                    suspendedUser,
                    _suspendedUsersFolder
                );
            }
            else
            {
                _logger.LogCritical(LogFormatterService.FormatAction(
                   actionLog,
                   "Suspended user is null!",
                   LogFormatterService.GetMethodName())
               );
            }
        }


        private int ParseToInt(string valuetToParse, int defaultValue)
        {
            int result = defaultValue;
            try
            {
                result = int.Parse(valuetToParse);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
            }

            return result;
        }
    }
}
