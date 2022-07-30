using System.ComponentModel.DataAnnotations;

namespace WebAppGoGlobal.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "No input Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "No input password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not correct")]
        public string ConfirmPassword { get; set; }
    }
}
