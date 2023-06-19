using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class SuspendedUserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int Attemps { get; set; }
        [Required]
        public DateTime DateFirstInvalidAttemp { get; set; }
        public DateTime? SuspendedDateTo { get; set; }
    }
}
