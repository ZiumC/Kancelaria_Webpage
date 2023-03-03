using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class JwtModel
    {
        [Required]
        public string AccessToken{ get; set; }
        [Required]
        public string RefreshToken{ get; set; }
    }
}
