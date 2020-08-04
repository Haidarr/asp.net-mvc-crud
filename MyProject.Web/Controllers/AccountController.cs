using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Web.Models;
using MyProject.Web.Services;


namespace MyProject.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly taskContext _taskContext;
  

        public AccountController(taskContext taskContext)
        {
            _taskContext = taskContext;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // If user already logged in, redirect to home.
            if (User.Identity.IsAuthenticated)
                return RedirectToHome();

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
      
                model.Username = model.Username?.ToLowerInvariant();
                // Hash password
                string hashedPassword = usersControl.HashPassword(model.Password);


                var users = (from us in _taskContext.Users
                            where us.login_name == model.Username 
                            && us.password == hashedPassword
                            && us.active == true
                            select us);

                var user = users.Include(u => u.Roles).FirstOrDefault();

                if (user != null)
                {
            
                    await userSignIn(user.id, user.full_name, user.Roles[0].role);

                    return RedirectToHome(user.id);
                    
                }
                else
                {
                    // If Username / Password not valid.
                    ModelState.AddModelError(string.Empty, "Invalid Username/Password. Please try again.");
                }
            }

            return View();
        }

  
        private async Task userSignIn(long id, string fullname, string role)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.SerialNumber, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, fullname));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task<IActionResult> Logout()
        {
            var id = User.Identity.GetUserID();
            var user = await _taskContext.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            // Sign out.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }


        private IActionResult RedirectToHome(long? id = null)
        {
            if (id == null)
                id = User.Identity.GetUserID();

            return RedirectToAction("Index", "Home", new { id });
        }

    }
}
