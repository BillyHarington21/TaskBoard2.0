using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public class AuthorisationService : IAuthorisationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthorisationService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("User already exists.");
            }            
            
            var RoleId = _roleRepository.GetRoleIdByNameAsync("User");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash =request.Password,
                RoleId = await RoleId,
                IsBlocked = false
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

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.PasswordHash = newPassword; 

            await _userRepository.UpdateAsync(user);

            return new ForgotPasswordResponse { Email = user.Email };
        }

        public async Task AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            user.Role = role;
            await _userRepository.UpdateAsync(user);
        }

        public async Task BlockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.IsBlocked = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task UnblockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.IsBlocked = false;
            await _userRepository.UpdateAsync(user);
        }
    } 
}
    

