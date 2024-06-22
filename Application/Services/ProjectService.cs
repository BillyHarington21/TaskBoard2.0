using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDTO> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
            };
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(project => new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description                
            });
        }

        public async Task AddAsync(ProjectDTO projectDto)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = projectDto.Name,
                Description = projectDto.Description              
            };

            await _projectRepository.AddAsync(project);
        }

        public async Task UpdateAsync(ProjectDTO projectDto)
        {
            var project = new Project
            {
                Id = projectDto.Id,
                Name = projectDto.Name,
                Description = projectDto.Description                
            };

            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }
    }
}
