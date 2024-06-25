using Domain.Entities;

namespace Domain.Repository
{
    public interface ITaskRepository
    {
        Task<TaskWork> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskWork>> GetAllAsync();
        Task AddAsync(TaskWork task);
        Task UpdateAsync(TaskWork task);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TaskWork>> GetAllBySprintIdAsync(Guid sprintId);
    }
}
