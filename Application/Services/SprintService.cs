using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintService(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<SprintDTO> CreateAsync(SprintDTO sprintDto)
        {
            var sprint = new Sprint
            {
                Id = Guid.NewGuid(),
                Name = sprintDto.Name,
                Description = sprintDto.Description,
                StartDate = sprintDto.StartDate,
                EndDate = sprintDto.EndDate,
                ProjectId = sprintDto.ProjectId
            };

            await _sprintRepository.AddAsync(sprint);

            return new SprintDTO
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Description = sprintDto.Description,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                ProjectId = sprint.ProjectId
            };
        }

        public async Task<SprintDTO> GetByIdAsync(Guid id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id);
            return new SprintDTO
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Description = sprint.Description,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                ProjectId = sprint.ProjectId
            };
        }

        public async Task<IEnumerable<SprintDTO>> GetAllByProjectIdAsync(Guid projectId)
        {
            var sprints = await _sprintRepository.GetAllByProjectIdAsync(projectId);
            return sprints.Select(sprint => new SprintDTO
            {
                Id = sprint.Id,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate
            });
        }

        public async Task UpdateAsync(SprintDTO sprintDto)
        {
            var sprint = await _sprintRepository.GetByIdAsync(sprintDto.Id);
            sprint.Name = sprintDto.Name;
            sprint.Description = sprintDto.Description;
            sprint.StartDate = sprintDto.StartDate;
            sprint.EndDate = sprintDto.EndDate;

            await _sprintRepository.UpdateAsync(sprint);
        }
        public async Task DeleteAsync(Guid id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id);
            if (sprint != null)
            {
                await _sprintRepository.DeleteAsync(id);
            }
        }
    }
}
