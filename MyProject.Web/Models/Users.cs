using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.Models
{
    public class UsersModel
    {
        [Display(Name = "User Name:")]
        public string login_name { get; set; }

        [Display(Name = "Password:")]
        public string password { get; set; }

        [Display(Name = "Full Name:")]
        public string full_name { get; set; }

        [Display(Name = "Role:")]
        public string role { get; set; }

        [Display(Name = "Status:")]
        public bool active { get; set; }
    }
}
