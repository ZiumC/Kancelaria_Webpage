using GB_Webpage.Models;
using Microsoft.EntityFrameworkCore;

namespace GB_Webpage.Data
{
    public class ApiContext : DbContext
    {

        public DbSet<ArticleModel> Articles { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
