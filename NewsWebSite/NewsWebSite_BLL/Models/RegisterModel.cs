using System.ComponentModel.DataAnnotations;

namespace NewsWebSite_BLL.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }
        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

    }
}
