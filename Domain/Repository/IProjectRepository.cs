using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
        System.Threading.Tasks.Task AddAsync(Project project);
        System.Threading.Tasks.Task UpdateAsync(Project project);
        System.Threading.Tasks.Task DeleteAsync(Guid id);
    }
}
