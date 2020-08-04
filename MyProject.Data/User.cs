using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Data
{
    [Table("users")]
    public class User
    {
        public User()
        {
            Roles = new List<UserRole>();
        }


        [Key]
        public long id { get; set; }

        public string login_name { get; set; }

        public string password { get; set; }

        public string full_name { get; set; }

        public bool active { get; set; }


        public List<UserRole> Roles { get; set; }
    }
}
