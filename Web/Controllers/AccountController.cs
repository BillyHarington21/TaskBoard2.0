﻿using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.AccountModels;
using Web.Models.UserModel;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthorisationService _authorisationService;
        private readonly IUserRepository _userRepository;

        public AccountController(IAuthorisationService authorisationService, IUserRepository userRepository)
        {
            _authorisationService = authorisationService;
            _userRepository = userRepository;
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
                    ConfirmPassword = model.ConfirmPassword,
                    
                };

                var response = await _authorisationService.RegisterAsync(dto);
                return RedirectToAction("Login", "Account");
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
                    Password = model.Password,
                    IsBlocked = model.IsBlocked

                };

                try
                {
                    var response = await _authorisationService.LoginAsync(dto);
                    if (response != null )
                    {
                        HttpContext.Session.SetString("UserEmail", response.Email);
                        HttpContext.Session.SetString("UserRole", response.RoleId.ToString());
                        var user = _userRepository.GetByEmailAsync(response.Email);
                        HttpContext.Session.SetString("UserId", user.Result.Id.ToString());
                        if (response.RoleId.ToString() == "5c200f10-64fc-48bb-a0a8-8f6008a124fa" || response.RoleId.ToString() == "26407a59-8b7f-4c0f-a534-edb962195abe")
                        {
                            return RedirectToAction("Index", "Project");
                        }
                        else return RedirectToAction("MySprints", "user");
                            
                    }
                    else if ( response.IsBlocked == true )
                    {
                        ModelState.AddModelError(string.Empty, "Your account has been blocked.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
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
               

        
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var model = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role.Name,
                IsBlocked = u.IsBlocked
            }).ToList();

            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> AssignRole(Guid userId, string roleName)
        {
            await _authorisationService.AssignRoleAsync(userId, roleName);
            return RedirectToAction("ManageUsers");
        }

        
        [HttpPost]
        public async Task<IActionResult> BlockUser(Guid userId)
        {
            await _authorisationService.BlockUserAsync(userId);
            return RedirectToAction("ManageUsers");
        }

        
        [HttpPost]
        public async Task<IActionResult> UnblockUser(Guid userId)
        {
            await _authorisationService.UnblockUserAsync(userId);
            return RedirectToAction("ManageUsers");
        }
    }
}
