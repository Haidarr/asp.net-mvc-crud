using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Enter your username.")]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
