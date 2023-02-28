using GB_Webpage.Data;
using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly IApiService _apiService;

        public ArticlesController(IApiService apiService)
        {
            _apiService = apiService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            ArticleModel model = await _apiService.GetAllArticles();

            return Ok(model);
        }
    }
}
