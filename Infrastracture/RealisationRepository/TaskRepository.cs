using Domain.Entities;
using Domain.Repository;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.RealisationRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public Task AddAsync(TaskWork task)
        {
            _context.Tasks.Add(task);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskWork>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<TaskWork>> GetAllBySprintIdAsync(Guid sprintId) 
        {
            return await _context.Tasks
                .Where(s => s.SprintId == sprintId)
                .ToListAsync();
        }

        public async Task<TaskWork> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task UpdateAsync(TaskWork task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

      
    }
}
