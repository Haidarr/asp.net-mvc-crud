using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Data
{
    [Table("projects")]
    public class Project
    {
        [Key]
        public long id { get; set; }
        public string project_name { get; set; }
        public string project_description { get; set; }
        public long assign_to { get; set; }
        public string project_deadline { get; set; }
        public DateTime time_updated { get; set; }
    }
}

