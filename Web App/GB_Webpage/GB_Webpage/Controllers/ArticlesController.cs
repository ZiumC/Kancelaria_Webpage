using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.DatabaseFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseFileService _databaseFileService;
        private readonly ILogger<ArticlesController> _logger;
        private readonly string _ArticlesFolder;
        private readonly Dictionary<int, string> statuses;

        private readonly int OK = 200, OK_NOT_SAVED = 209, BAD_REQUEST = 400, NOT_FOUND = 404;

        public ArticlesController(ILogger<ArticlesController> logger, IDatabaseFileService databaseFileService, IApiService apiService, IConfiguration configuration)
        {
            _databaseFileService = databaseFileService;
            _logger = logger;
            _configuration = configuration;
            _apiService = apiService;
            _ArticlesFolder = _configuration["DatabaseStorage:ArticlesFolder"];

            statuses = new Dictionary<int, string>()
            {
                { 200, "ok." },
                { 209, "changes not saved in physical file." },
                { 400, "something went wrong with article." },
                { 404, "article not found." }
            };
        }

        [HttpPut]
        [Route("update/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDTO newArticle)
        {
            string actionLog = "Updating article.";

            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            ArticleModel? oldArticle = await _apiService.GetArticleByIdAsync(id);
            if (oldArticle == null)
            {
                var statusResponse = SelectStatusBy(NOT_FOUND);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (id={id}).",
                    LogFormatterService.GetAsyncMethodName())
                );

                return NotFound($"Unable to find article. | {id}");
            }

            bool isUpdated = await _apiService.UpdateArticleByIdAsync(id, newArticle);
            if (isUpdated)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok("Article has been updated. Changes HAS BEEN SAVED to physical file");
                }
                else
                {
                    var statusResponse = SelectStatusBy(OK_NOT_SAVED);

                    _logger.LogWarning(LogFormatterService.FormatAction(
                        actionLog,
                        $"StatusCode={statusResponse.Item1}  -  {statusResponse.Item2} (id={id}).",
                        LogFormatterService.GetAsyncMethodName())
                    );

                    return StatusCode(209, "Article has been updated. Changes HAVEN'T BEEN SAVED to physical file");
                }
            }
            else
            {
                var statusResponse = SelectStatusBy(BAD_REQUEST);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1}  -  {statusResponse.Item2} (id={id}).",
                    LogFormatterService.GetAsyncMethodName())
                );

                return BadRequest($"Unable to update article. | {id}");
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            string actionLog = "Deleting article.";

            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            ArticleModel? article = await _apiService.GetArticleByIdAsync(id);
            if (article == null)
            {
                var statusResponse = SelectStatusBy(NOT_FOUND);

                _logger.LogInformation(LogFormatterService.FormatAction(actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (id={id}).",
                    LogFormatterService.GetAsyncMethodName())
                );

                return NotFound($"Article not found. | {id}");
            }

            bool isDeleted = await _apiService.DeleteArticleAsync(article);
            if (isDeleted)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok($"Article has been deleted. Changes HAS BEEN SAVED to physical file.");
                }
                else
                {
                    var statusResponse = SelectStatusBy(OK_NOT_SAVED);

                    _logger.LogWarning(LogFormatterService.FormatAction(actionLog,
                        $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (id={id}).",
                        LogFormatterService.GetAsyncMethodName())
                    );

                    return StatusCode(209, $"Article has been deleted. Changes HAVEN'T BEEN SAVED to physical file.");
                }
            }
            else
            {
                var statusResponse = SelectStatusBy(BAD_REQUEST);

                _logger.LogWarning(LogFormatterService.FormatAction(actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2} (id={id}).",
                    LogFormatterService.GetAsyncMethodName())
                );

                return BadRequest($"Unable to delete article. | {id}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddArticle(ArticleDTO articleDTO)
        {
            string actionLog = "Adding article.";

            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            bool isAdded = await _apiService.AddArticleAsync(articleDTO);
            if (isAdded)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok("Article has been added and SAVED to physical file");
                }
                else
                {
                    var statusResponse = SelectStatusBy(OK_NOT_SAVED);

                    _logger.LogWarning(LogFormatterService.FormatAction(
                        actionLog,
                         $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                        LogFormatterService.GetAsyncMethodName())
                    );

                    return StatusCode(209, "Article has been added and NOT SAVED to physical file");
                }
            }
            else
            {
                var statusResponse = SelectStatusBy(BAD_REQUEST);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetAsyncMethodName())
                );

                return BadRequest("Article haven't been added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            string actionLog = "Get Articles.";

            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            IEnumerable<ArticleModel> articles = await _apiService.GetAllArticlesAsync();
            if (articles != null)
            {
                return Ok(articles);
            }
            else
            {
                var statusResponse = SelectStatusBy(BAD_REQUEST);

                _logger.LogWarning(LogFormatterService.FormatAction(
                    actionLog,
                    $"StatusCode={statusResponse.Item1} - {statusResponse.Item2}.",
                    LogFormatterService.GetAsyncMethodName())
                );

                return NotFound("Articles not found");
            }
        }

        private Tuple<int, string> SelectStatusBy(int statusCode)
        {
            var selectedStatus = statuses
                        .Where(s => s.Key == statusCode)
                        .FirstOrDefault();

            return Tuple.Create(selectedStatus.Key, selectedStatus.Value);
        }
    }
}
