using System.ComponentModel.DataAnnotations;
namespace WebAppGoGlobal.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "No input Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "No input password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
