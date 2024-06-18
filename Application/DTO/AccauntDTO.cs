using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class RegisterRequest
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
    }

    public class RegisterResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsBlocked { get; set; } 
        public Guid RoleId { get; set; }
    }

    public class LoginResponse
    {
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordResponse
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
