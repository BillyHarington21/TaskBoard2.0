using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Infrastracture.RealisationRepository;
using Microsoft.Data.SqlClient;
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
        private readonly IUserRepository _userRepository;

        public SprintService(ISprintRepository sprintRepository, IUserRepository userRepository)
        {
            _sprintRepository = sprintRepository;
            _userRepository = userRepository;
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
            if (sprint == null)
            {
                return null;
            }

            return new SprintDTO
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Description = sprint.Description,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                ProjectId = sprint.ProjectId,
                AssignedUserIds = sprint.Users?.Select(u => u.Id).ToList() ?? new List<Guid>()
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
                EndDate = sprint.EndDate,
                Users = sprint.Users?.Select(user => new UserDTO
                {
                    Id = user.Id,
                    UserName = user.Email,
                }).ToList() ?? new List<UserDTO>()
            });
        }

        public async Task UpdateAsync(SprintDTO sprintDto)
        {
            var sprint = await _sprintRepository.GetByIdAsync(sprintDto.Id);
            if (sprint == null)
            {
                throw new Exception("Sprint not found.");
            }

            sprint.Name = sprintDto.Name;
            sprint.Description = sprintDto.Description;
            sprint.StartDate = sprintDto.StartDate;
            sprint.EndDate = sprintDto.EndDate;

            // Обновление пользователей
            
            foreach (var userId in sprintDto.AssignedUserIds)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    sprint.Users.Add(user);
                }
            }

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
        public async Task AssignUserToSprint(Guid sprintId, Guid userId)
        {
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            var user = await _userRepository.GetByIdAsync(userId);

            if (sprint != null && user != null)
            {
                if (sprint.Users == null)
                {
                    sprint.Users = new List<User>();
                }

                if (!sprint.Users.Any(u => u.Id == userId))
                {
                    sprint.Users.Add(user);

                    // Попытка добавления записи в таблицу связей
                    try
                    {
                        var sprintUser = new SprintUser
                        {
                            SprintId = sprintId,
                            UserId = userId
                        };
                        await _sprintRepository.AddSprintUserAsync(sprintUser);
                    }
                    catch (SqlException ex) when (ex.Number == 2627) // Ошибка дублирования ключа
                    {
                        // Логирование или обработка исключения при необходимости
                    }

                    await _sprintRepository.UpdateAsync(sprint);
                }
            }
        }
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.Email,
                RoleName = u.Role.Name,
                
            }).ToList();
        }
        public async Task RemoveUserFromSprint(Guid sprintId, Guid userId)
        {
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint != null)
            {
                var user = sprint.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    sprint.Users.Remove(user);
                    await _sprintRepository.RemoveSprintUserAsync(sprintId, userId);
                    await _sprintRepository.UpdateAsync(sprint);
                }
            }
        }
        public async Task<List<Guid>> GetAssignedUserIdsAsync(Guid sprintId)
        {
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                return new List<Guid>();
            }

            return sprint.Users.Select(u => u.Id).ToList();
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsersBySprintIdAsync(Guid sprintId)
        {
            var users = await _sprintRepository.GetUsersBySprintIdAsync(sprintId);
            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.Email
                // Добавьте здесь другие необходимые свойства
            });

            return userDtos;
        }
    }
}
