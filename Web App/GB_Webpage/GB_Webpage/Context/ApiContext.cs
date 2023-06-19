using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.Database.DatabaseFiles;
using Microsoft.EntityFrameworkCore;

namespace GB_Webpage.Data
{
    public class ApiContext : DbContext
    {

        private readonly IDatabaseFileService _databaseFileService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiContext> _logger;

        private readonly string _suspendedUsersFolder;
        private readonly string _articlesFolder;

        public ApiContext(ILogger<ApiContext> logger, DbContextOptions<ApiContext> options, IConfiguration configuration, IDatabaseFileService databaseFileService) : base(options)
        {
            _databaseFileService = databaseFileService;
            _configuration = configuration;
            _logger = logger;

            _suspendedUsersFolder = _configuration["Paths:DatabaseStorage:SuspendedUsersFolder"];
            _articlesFolder = _configuration["Paths:DatabaseStorage:ArticlesFolder"];
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
                _logger.LogInformation(LogFormatterService.FormatAction("List of articles is empty", null, LogFormatterService.GetMethodName()));
            }

            List<SuspendedUserModel>? suspendedUsers = _databaseFileService.ReadFile<List<SuspendedUserModel>>(_suspendedUsersFolder);
            if (suspendedUsers != null && suspendedUsers.Count() > 0)
            {
                modelBuilder.Entity<SuspendedUserModel>(usr =>
                {

                    foreach (SuspendedUserModel user in suspendedUsers)
                    {
                        usr.HasData(new SuspendedUserModel
                        {
                            Id = user.Id,
                            Username = user.Username,
                            DateFirstInvalidAttemp = user.DateFirstInvalidAttemp,
                            SuspendedDateTo = user.SuspendedDateTo,
                            Attempts = user.Attempts
                        });
                    }
                });
            }
            else
            {
                _logger.LogInformation(LogFormatterService.FormatAction("List suspended users is empty", null, LogFormatterService.GetMethodName()));
            }
        }

        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<SuspendedUserModel> BlockedUsers { get; set; }
    }
}
