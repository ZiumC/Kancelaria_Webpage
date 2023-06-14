using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class BlockedUserModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int TotalAttemps { get; set; }
        [Required]
        public DateTime BlockedFrom { get; set; }
        public DateTime BlockedTo { get; set; }
    }
}
