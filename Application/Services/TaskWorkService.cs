using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public class TaskWorkService : ITaskWorkService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskWorkService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskWorkDTO> CreateAsync(TaskWorkDTO taskDto)
        {
            var task = new TaskWork
            {
                Id = Guid.NewGuid(),
                Name = taskDto.Name,
                Description = taskDto.Description,
                Status = taskDto.Status,
                SprintId = taskDto.SprintId
            };

            await _taskRepository.AddAsync(task);

            return new TaskWorkDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId
            };
        }

        public async Task<TaskWorkDTO> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return new TaskWorkDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId
            };
        }

        public async Task<IEnumerable<TaskWorkDTO>> GetAllBySprintIdAsync(Guid sprintId)
        {
            var tasks = await _taskRepository.GetAllBySprintIdAsync(sprintId);
            return tasks.Select(task => new TaskWorkDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId
            });
        }

        public async Task UpdateAsync(TaskWorkDTO taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskDto.Id);
            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                await _taskRepository.DeleteAsync(id);
            }
        }
    }
}

