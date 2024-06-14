using Domain.Entities;
using Domain.Repository;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.RealisationRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new Exception($"Role '{roleName}' not found");
            }
            return role.Id;
        }
    }

}
