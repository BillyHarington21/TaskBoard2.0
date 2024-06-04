using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Infrastracture.RealisationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AutorisationService : IAutorisationService
    {
        private readonly IUserRepository _userRepository;

        public AutorisationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = request.Password // For simplicity, storing password as plain text
            };

            await _userRepository.AddAsync(user);

            return new RegisterResponse { UserId = user.Id.ToString(), Email = user.Email };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || user.PasswordHash != request.Password)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return new LoginResponse { Email = user.Email };
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var newPassword = "newpassword"; // For simplicity, using a fixed new password
            user.PasswordHash = newPassword;

            await _userRepository.UpdateAsync(user);

            return new ForgotPasswordResponse { Email = user.Email, NewPassword = newPassword };
        }
    } 
}
    

