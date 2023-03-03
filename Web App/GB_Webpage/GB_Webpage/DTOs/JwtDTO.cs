using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.DTOs
{
    public class JwtDTO
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
