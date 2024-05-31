using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITaskRepository
    {
        Task<Task> GetByIdAsync(Guid id);
        Task<IEnumerable<Task>> GetAllAsync();
        Task AddAsync(Task task);
        Task UpdateAsync(Task task);
        Task DeleteAsync(Guid id);
    }
}
