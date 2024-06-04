using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITaskRepository
    {
        Task<TaskWork> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskWork>> GetAllAsync();
        Task AddAsync(TaskWork task);
        Task UpdateAsync(TaskWork task);
        Task DeleteAsync(Guid id);
    }
}
