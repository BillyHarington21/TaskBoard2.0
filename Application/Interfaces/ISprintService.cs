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
        Task AssignUserToSprint(Guid sprintId, Guid userId);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<Guid>> GetAssignedUserIdsAsync(Guid sprintId);
        Task RemoveUserFromSprint(Guid sprintId, Guid userId);
        Task<IEnumerable<UserDTO>> GetAllUsersBySprintIdAsync(Guid sprintId);
    }
}
