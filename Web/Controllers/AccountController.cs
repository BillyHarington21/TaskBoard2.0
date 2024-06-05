using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authorisationService.RegisterAsync(model);
                // Handle successful registration (e.g., redirect to login page)
                return RedirectToAction("Login");
            }
            return View(model);
        }
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authorisationService.LoginAsync(model);
                    // Handle successful login (e.g., set authentication cookie)
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
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model, string newPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authorisationService.ForgotPasswordAsync(model, newPassword);
                    // Handle successful password reset (e.g., display a success message)
                    ViewBag.Message = "Your password has been successfully reset.";
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Failed to reset password.");
                }
            }
            return View(model);
        }
    }
}
