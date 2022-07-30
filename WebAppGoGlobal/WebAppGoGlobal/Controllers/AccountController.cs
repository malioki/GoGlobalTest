using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppGoGlobal.Data;
using WebAppGoGlobal.Models;
using WebAppGoGlobal.ViewModels;

namespace WebAppGoGlobal.Controllers
{
    public class AccountController : Controller
    {
        private BookmarkContext _context;
        private string ReturnUrl { get; set; } = String.Empty;

        public AccountController(BookmarkContext condext, string returnUrl = "")
        {
            this._context = condext;
            this.ReturnUrl = returnUrl;
        }

        [HttpGet("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    _context.Users.Add(new User { Email = model.Email, Password = model.Password });
                    await _context.SaveChangesAsync();

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Bookmark");
                }
                else
                    ModelState.AddModelError("", "Incorrect Login and(or) password");
            }
            return View(nameof(ReturnUrl), model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Route(template: "Login", Name = "ReturnUrl")]
        public IActionResult Login(string returnUrl)
        {
            this.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Bookmark");
                }
                ModelState.AddModelError("", "Incorrect Login and(or) password");
            }
            return View(nameof(ReturnUrl), model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
