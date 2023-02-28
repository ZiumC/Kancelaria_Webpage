using GB_Webpage.Models;
using Microsoft.EntityFrameworkCore;

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
        }

        public DbSet<ArticleModel> Articles { get; set; }
    }
}
