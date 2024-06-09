using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthorisationService _authorisationService;

        public AccountController(IAuthorisationService authorisationService) 
        {
            _authorisationService = authorisationService;
        }
        
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new RegisterRequest
                {
                    
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                var response = await _authorisationService.RegisterAsync(dto);
                return RedirectToAction("Login");
            }
            return View(model);
        }
        // POST: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new LoginRequest
                {
                    Email = model.Email,
                    Password = model.Password
                };

                try
                {
                    var response = await _authorisationService.LoginAsync(dto);
                    HttpContext.Session.SetString("UserEmail", response.Email);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        // GET: Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError(string.Empty, "New password and confirmation password do not match.");
                    return View(model);
                }

                var dto = new ForgotPasswordRequest
                {
                    Email = model.Email
                };

                try
                {
                    var response = await _authorisationService.ForgotPasswordAsync(dto, model.NewPassword);
                    return RedirectToAction("Login");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Failed to reset password.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
