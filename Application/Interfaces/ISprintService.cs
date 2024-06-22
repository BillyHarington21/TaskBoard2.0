using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISprintService
    {
        Task<SprintDTO> CreateAsync(SprintDTO sprintDto);
        Task<SprintDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<SprintDTO>> GetAllByProjectIdAsync(Guid projectId);
        Task UpdateAsync(SprintDTO sprintDto);
        Task DeleteAsync(Guid id);
    }
}
