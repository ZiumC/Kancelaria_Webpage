using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class BlockedUserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int TotalAttemps { get; set; }
        [Required]
        public DateTime DateFirstInvalidAttemp { get; set; }
        public DateTime? DateBlockedTo { get; set; }
    }
}
