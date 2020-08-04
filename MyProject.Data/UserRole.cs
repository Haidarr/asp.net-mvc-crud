using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Data
{
    [Table("user_role")]
    public class UserRole
    {

        public long user_id { get; set; }

        [ForeignKey(nameof(user_id))]
        public User User { get; set; }

        public string role { get; set; }
    }
}
