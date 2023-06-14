using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.EntityFrameworkCore;

namespace GB_Webpage.Data
{
    public class ApiContext : DbContext
    {

        private readonly string _folder;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiContext> _logger;

        public ApiContext(ILogger<ApiContext> logger, DbContextOptions<ApiContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            _folder = _configuration["DatabaseStorage:ArticlesFolder"];
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //List<ArticleModel>? articles = new DatabaseFileService(_folder).ReadFile<List<ArticleModel>>();
            List<ArticleModel>? articles = null;

            if (articles != null)
            {

                if (articles.Count > 0)
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
            }

        }

        public DbSet<ArticleModel> Articles { get; set; }
    }
}
