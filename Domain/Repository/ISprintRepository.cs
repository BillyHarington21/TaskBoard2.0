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
        Task AddAsync(Sprint sprint);
        Task UpdateAsync(Sprint sprint);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Sprint>> GetAllByProjectIdAsync(Guid projectId);

    }
}
