using AuthenticationTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM mode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(mode.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Emial", "This Emila is already Existed");
                    return View(mode);
                }

                var res = await _userManager.CreateAsync(
                     new AppUser
                     {
                         Email = mode.Email,
                         UserName = mode.Email.Split("@")[0],
                         FName = mode.FirstName,
                         LName = mode.LastName,
                     }, mode.Password
                    );  
                if (res.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                foreach (var Erorr in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, Erorr.Description); 
                }
            }
            return View(mode);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInVM mode)
        
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(mode.Email);
                if (user is null)
                {
                    ModelState.AddModelError("Email", "InVaild Eamil");
                    return View(mode);
                }
                var res = await _signInManager.CheckPasswordSignInAsync(user, mode.Password, false);
                if (res.Succeeded)
                {
                   
                    var res2 =  await _signInManager.PasswordSignInAsync(user, mode.Password, mode.RememberMe, false);
                    if (res2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                }
                ModelState.AddModelError("Password", "InVaild Password");
            }
            return View(mode);
        }

        public async Task<IActionResult> SinOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn)); 
        }
        [Authorize]
        public async Task<IActionResult> ProFile()
        {
            var emile = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(emile);
            var userVm = new UserVM
            {
                FirstName = user.FName,
                LastName = user.LName,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(userVm);
        }
    }
}
