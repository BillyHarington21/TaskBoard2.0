using System.ComponentModel.DataAnnotations;

namespace Web.Models.AccountModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
    }
}
