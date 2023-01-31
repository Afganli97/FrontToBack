using System;
using System.Threading.Tasks;
using FrontToBack.Helpers;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login, string ReturnUrl)
        {   
            if(!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if(user == null)
            {
                ModelState.AddModelError("","Email or password wrong!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user,login.Password, login.RememberMe, true);
            
            if(result.IsLockedOut)
            {
                ModelState.AddModelError("Lockout", "You are blocked! Please try again later.");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","Email or password wrong!");
                return View();
            }

            await _signInManager.SignInAsync(user, login.RememberMe);

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if(!ModelState.IsValid) return View();

            AppUser user = new AppUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                FullName = register.FullName
            };

            IdentityResult result =  await _userManager.CreateAsync(user, register.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View();
            }

            await _userManager.AddToRoleAsync(user,RolesEnum.Member.ToString());

            return RedirectToAction("index", "home");
        }

        // public async Task<IActionResult> AddRoles()
        // {
        //     foreach (var role in Enum.GetValues(typeof(RolesEnum)))
        //     {
        //         if(!await _roleManager.RoleExistsAsync(role.ToString()))
        //         {
        //             await _roleManager.CreateAsync(new IdentityRole{Name = role.ToString()});
        //         }
        //     }
        //     return Content("Role elave olundu");
        // }
    }
}