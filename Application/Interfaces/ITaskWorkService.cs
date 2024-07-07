using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITaskWorkService
    {

        Task<TaskWorkDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskWorkDTO>> GetAllBySprintIdAsync(Guid sprintId);
        Task<TaskWorkDTO> CreateAsync(TaskWorkDTO taskDto);
        Task UpdateAsync(TaskWorkDTO taskDto);
        Task DeleteAsync(Guid id);
        Task<UserDTO> GetUserAsync(Guid? id);
        
    }
}
