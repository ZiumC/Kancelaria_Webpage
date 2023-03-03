using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
