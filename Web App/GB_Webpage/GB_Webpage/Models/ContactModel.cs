using System.ComponentModel.DataAnnotations;

namespace GB_Webpage.Models
{
    public class ContactModel
    {

        [Required(), MinLength(2), MaxLength(50)]
        [RegularExpression(@"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")]
        public string Name { get; set; }

        [Required(), MinLength(2), MaxLength(50)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required(), MinLength(10), MaxLength(5000)]
        public string Message { get; set; }
    }
}
