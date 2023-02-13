using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class EmailForm
    {

        [Required()]
        public string Name { get; set; }

        [Required()]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required()]
        public string Message { get; set; } 
    }
}
