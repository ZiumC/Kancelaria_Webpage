using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.DTOs
{
    public class ArticleDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
