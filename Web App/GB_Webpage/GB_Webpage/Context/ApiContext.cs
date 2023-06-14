using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.DatabaseFiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GB_Webpage.Data
{
    public class ApiContext : DbContext
    {

        private readonly string _articlesFolder;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiContext> _logger;
        private readonly IDatabaseFileService _databaseFileService;

        public ApiContext(ILogger<ApiContext> logger, DbContextOptions<ApiContext> options, IConfiguration configuration, IDatabaseFileService databaseFileService) : base(options)
        {
            _configuration = configuration;
            _articlesFolder = _configuration["Paths:DatabaseStorage:ArticlesFolder"];
            _databaseFileService = databaseFileService;
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<ArticleModel>? articles = _databaseFileService.ReadFile<List<ArticleModel>>(_articlesFolder);

            if (articles != null && articles.Count > 0)
            {
                modelBuilder.Entity<ArticleModel>(art =>
                {

                    foreach (ArticleModel articleItem in articles)
                    {
                        art.HasData(new ArticleModel
                        {
                            Id = articleItem.Id,
                            Title = articleItem.Title,
                            Description = articleItem.Description,
                            Date = articleItem.Date
                        });
                    }
                });
            }
            else
            {
                _logger.LogInformation(LogFormatterService.FormatAction("List of articles is empty", null, MethodBase.GetCurrentMethod()?.Name));
            }

        }

        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<BlockedUserModel> BlockedUsers { get; set; }
    }
}
