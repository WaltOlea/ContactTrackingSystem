using ContactPage.Models;
using ContactPage.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactPage.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ContactDbContext _contactDbContext;

        public AccountController(ContactDbContext contactDbContext)
        {
            _contactDbContext = contactDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();

            // Set the lading page to Contact
            model.ReturnUrl = "/Home/Contact";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _contactDbContext.Users.Where(c => c.UserName == model.UserName && c.Password == model.Password).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("Error", "Incorrect User Name or Password");
                return View(model);
            }
            else
            {
                // Add Validated User to the Claim and Principle to set the Authorize Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Role, string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true });

                return LocalRedirect(model.ReturnUrl);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}