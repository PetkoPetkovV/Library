using Library.Data.Models;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(SignInManager<ApplicationUser> _signInManager, 
            UserManager<ApplicationUser> _userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() 
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Books");
            }

            var model = new RegisterViewModel();

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var appUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "User");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }

            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]

        public IActionResult Login() 
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Books");
            }

            var model = new LoginViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var appUser = await userManager.FindByNameAsync(model.UserName);

            if (appUser != null)
            {
                var result = await signInManager.PasswordSignInAsync(appUser, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("All", "Books");
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
