using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ISprintRepository
    {
        Task<Sprint> GetByIdAsync(Guid id);
        Task<IEnumerable<Sprint>> GetAllAsync();
        System.Threading.Tasks.Task AddAsync(Sprint sprint);
        System.Threading.Tasks.Task UpdateAsync(Sprint sprint);
        System.Threading.Tasks.Task DeleteAsync(Guid id);
    }
}
