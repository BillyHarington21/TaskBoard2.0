using Domain.Entities;
using Domain.Repository;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.RealisationRepository
{
    public class SprintRepository : ISprintRepository
    {
        private readonly ApplicationDbContext _context;
        public SprintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Sprint sprint)
        {
            _context.Sprints.Add(sprint);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint != null)
            {
                _context.Sprints.Remove(sprint);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Sprint>> GetAllAsync()
        {
            return await _context.Sprints.ToListAsync();
        }

        public async Task<Sprint> GetByIdAsync(Guid id)
        {
            return await _context.Sprints.FindAsync(id);
        }

        public async Task UpdateAsync(Sprint sprint)
        {
            _context.Sprints.Update(sprint);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Sprint>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.Sprints
                .Where(s => s.ProjectId == projectId)
                .ToListAsync();
        }
    }

   
}
