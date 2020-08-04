using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.Models
{
    public class ProjectsModel
    {

        [Display(Name = "Project/Task Name:")]
        public string project_name { get; set; }

        [Display(Name = "Assign to:")]
        public long assign_to { get; set; }

        [Display(Name = "Project/Task Description:")]
        public string project_description { get; set; }

        [Display(Name = "Deadline:")]
        public string project_deadline { get; set; }
    }
}
