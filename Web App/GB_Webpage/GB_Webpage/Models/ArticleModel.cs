using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class ArticleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
