using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectDTO>> GetAllAsync();
        Task AddAsync(ProjectDTO project);
        Task UpdateAsync(ProjectDTO project);
        Task DeleteAsync(Guid id);
    }
}
