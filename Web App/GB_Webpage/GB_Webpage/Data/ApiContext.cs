using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GB_Webpage.Data
{
    public class ApiContext : DbContext
    {


        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<ArticleModel> articles = new DatabaseFileService().ReadFile<List<ArticleModel>>();

            if (articles != null)
            {

                if (articles.Count > 0)
                {

                    modelBuilder.Entity<ArticleModel>(art =>
                    {

                        foreach (ArticleModel articleItem in articles)
                        {
                            art.HasData(new ArticleModel { Id = articleItem.Id, Title = articleItem.Title, Description = articleItem.Description, Date = articleItem.Date });
                        }
                    });
                }
            }

        }

        public DbSet<ArticleModel> Articles { get; set; }
    }
}
