using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
