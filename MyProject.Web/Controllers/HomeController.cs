using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Web.Models;


namespace MyProject.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly taskContext _taskContext;

        public HomeController(taskContext taskContext)
        {
            _taskContext = taskContext;
        }

        public IActionResult Index()
        {
            var UserCurrentRole = User.Identity.GetUserCurrentRole();

            ViewBag.allUsers = UsersList().ToList();
            

            if (UserCurrentRole == RoleType.ADMIN)
            {
                // Load all tasks if the user is Admin
                ViewBag.allTasks = allTasksList().ToList();
            }
            else {
                // Load assigned tasks to this user
                ViewBag.allTasks = userTasksList().ToList();
            }

            return View();
        }


        public IActionResult Create()
        {
            // get all Staff users
            ViewBag.allUsers = UsersList().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectsModel model)
        {
            if (ModelState.IsValid)
            {
                // Save form values into dataContext.
                var newTask = new Project();
                newTask.project_name =  model.project_name;
                newTask.project_description = model.project_description;
                newTask.assign_to = model.assign_to;
                newTask.project_deadline = model.project_deadline;
                newTask.time_updated = DateTime.Now;

           
                _taskContext.Add(newTask);
                _taskContext.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.allUsers = UsersList().ToList();

            return View(model);
        }


        public IActionResult Update(int id)
        {
            var UserCurrentRole = User.Identity.GetUserCurrentRole();
           
            Project task = _taskContext.Projects.FirstOrDefault(t => t.id == id);

            if (task == null || UserCurrentRole == RoleType.Staff) {
                return RedirectToAction("Index");
            }

            ViewBag.projectId = id;
            ViewBag.allUsers = UsersList().ToList();

            ProjectsModel model = new ProjectsModel();

            model.project_name = task.project_name;
            model.project_description = task.project_description;
            model.assign_to = task.assign_to;
            model.project_deadline = task.project_deadline;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProjectsModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Project task = _taskContext.Projects.FirstOrDefault(x => x.id == id);

                task.project_name = model.project_name;
                task.project_description = model.project_description;
                task.assign_to = model.assign_to;
                task.project_deadline = model.project_deadline;

                _taskContext.SaveChanges();

                return RedirectToAction("Index");

            }

            ViewBag.allUsers = UsersList().ToList();

            return View(model);
        }


        public IActionResult View(int id)
        {
            ViewBag.projectId = id;
            ViewBag.allUsers = UsersList().ToList();

            Project task = _taskContext.Projects.FirstOrDefault(x => x.id == id);

            if (task == null)
            {
                return RedirectToAction("Index");
            }

            ProjectsModel model = new ProjectsModel();

            model.project_name = task.project_name;
            model.project_description = task.project_description;
            model.assign_to = task.assign_to;
            model.project_deadline = task.project_deadline;

            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {

            Project task = _taskContext.Projects.FirstOrDefault(x => x.id == id);

            _taskContext.Remove(task);
            _taskContext.SaveChanges();

            return NoContent();

        }


        private IQueryable<Project> allTasksList()
        {
          
            var Projetcs = (from tasks in _taskContext.Projects
                            select tasks);
          
            
            return Projetcs;

        }

        private IQueryable<Project> userTasksList()
        {

            var Projetcs = (from tasks in _taskContext.Projects
                            where tasks.assign_to == User.Identity.GetUserID()
                            select tasks);

            return Projetcs;

        }


        private IQueryable<User> UsersList()
        {
            var allUsers = (from user in _taskContext.Users.Include(x => x.Roles)
                            where user.Roles[0].role != RoleType.ADMIN 
                              select user);


            return allUsers;

        }
    }
}
