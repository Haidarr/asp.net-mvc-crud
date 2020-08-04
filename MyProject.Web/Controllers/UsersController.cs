using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Web.Models;
using MyProject.Web.Services;

namespace MyProject.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly taskContext _taskContext;

        public UsersController(taskContext taskContext)
        {
            _taskContext = taskContext;
        }

        public IActionResult Index()
        {
            var UserCurrentRole = User.Identity.GetUserCurrentRole();
            if (UserCurrentRole != RoleType.ADMIN)
            {
                // If user role is not Admin, go to Home 
                return RedirectToAction("Index", "Home");
            }

            ViewBag.allUsers = getUsers().ToList();

            return View();
        }

        public IActionResult Create()
        {
          
            if (User.Identity.GetUserCurrentRole() != RoleType.ADMIN)
            {
                // Redirect to Home, if the current User Role is not Admin
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                
                model.login_name = model.login_name?.ToLowerInvariant();

                // Check for duplicate user name.
                var duplicateUsers = from x in _taskContext.Users
                                     where x.login_name.ToLower() == model.login_name.ToLower()
                                     select x;

                if (!duplicateUsers.Any())
                {
                    // Add User.
                    var newUser = new User();
                    newUser.login_name = model.login_name;
                    newUser.password = usersControl.HashPassword(model.password);
                    newUser.full_name = model.full_name;
                    newUser.active = model.active;

                    if (model.role == RoleType.ADMIN) {
                        newUser.Roles.Add(new UserRole { role = RoleType.ADMIN });
                    }

                    if (model.role == RoleType.Staff)
                    {
                        newUser.Roles.Add(new UserRole { role = RoleType.Staff });
                    }


                    if (newUser.Roles.Any())
                    {
                        // Save changes to DB.
                        _taskContext.Add(newUser);
                        _taskContext.SaveChanges();

                        return RedirectToAction("Index");
                    }
                   
                }
                else
                {
                    ModelState.AddModelError("", "This Username is already exsit. Please choose different username.");
                }

            }

            return View(model);
        }

        public IActionResult Update(int id)
        {

            var UserCurrentRole = User.Identity.GetUserCurrentRole();
            var UserCurrentID = User.Identity.GetUserID();

            if (UserCurrentRole != RoleType.ADMIN && UserCurrentID != id)
            {
                // Redirect to Home, if the current User Role is not Admin
                return RedirectToAction("Index", "Home");
            }

            User user = _taskContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.id == id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            UsersModel model = new UsersModel();
            model.login_name = user.login_name;
            model.full_name = user.full_name;
            model.active = user.active;

            if (user.Roles.Any(x => x.role == RoleType.ADMIN)) {
                model.role = RoleType.ADMIN;
            }

            if (user.Roles.Any(x => x.role == RoleType.Staff))
            {
                model.role = RoleType.Staff;
            }
                    
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UsersModel model, int id) 
        {
            if (ModelState.IsValid)
            {
                var UserCurrentRole = User.Identity.GetUserCurrentRole();
                
                model.login_name = model.login_name?.ToLowerInvariant();

                // Check for duplicate login name.
                var duplicateUsers = from x in _taskContext.Users
                                     where x.login_name.ToLower() == model.login_name.ToLower()
                                     && x.id != id
                                     select x;

                User user = _taskContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.id == id);

                if (UserCurrentRole == RoleType.ADMIN)
                {
                    if (!duplicateUsers.Any())
                    {
                        // Save values into DB.
                        user.login_name = model.login_name;
                        user.password = (model.password == null) ? user.password : usersControl.HashPassword(model.password);
                        user.full_name = model.full_name;
                        user.active = model.active;

                        if (model.role == RoleType.ADMIN)
                        {
                            if (!user.Roles.Any(u => u.role == RoleType.ADMIN))
                            {
                                user.Roles.Add(new UserRole { role = RoleType.ADMIN });
                            }
                        }
                        else if (user.Roles.Any(u => u.role == RoleType.ADMIN))
                        {
                            _taskContext.Remove(user.Roles.First(u => u.role == RoleType.ADMIN));
                        }

                
                        if (model.role == RoleType.Staff)
                        {
                            if (!user.Roles.Any(u => u.role == RoleType.Staff))
                            {
                                user.Roles.Add(new UserRole { role = RoleType.Staff });
                            }
                        }
                        else if (user.Roles.Any(u => u.role == RoleType.Staff))
                        {
                            _taskContext.Remove(user.Roles.First(u => u.role == RoleType.Staff));
                        }


                        if (user.Roles.Any())
                        {
                            // Save to DB.
                            _taskContext.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "The User Name is already exsit.");
                    }

                }

                else {
                    user.password = (model.password == null) ? user.password : usersControl.HashPassword(model.password);
                    user.full_name = model.full_name;

                    // Save to DB.
                    _taskContext.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check if the user has any Tasks 
            var hasTask = from u in _taskContext.Projects
                            where u.assign_to == id
                            select u;

            
            if (id == User.Identity.GetUserID() || hasTask.Any())
            {
                // Cannot delete this User (if he has Tasks or he cannot delete himself).
                return BadRequest();
            }
            else
            {
                User user = _taskContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.id == id);

                // Delete user's role
                foreach (var role in user.Roles)
                    _taskContext.Remove(role);
 
                _taskContext.Remove(user);
                _taskContext.SaveChanges();

                return NoContent();

            }
        }

        private IQueryable<User> getUsers()
        {
            var allUsers = (from ve in _taskContext.Users.Include(x => x.Roles)
                            select ve);
            return allUsers;

        }
    }
}
