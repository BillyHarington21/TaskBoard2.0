﻿using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public class TaskWorkService : ITaskWorkService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskWorkService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskWorkDTO> CreateAsync(TaskWorkDTO taskDto)
        {
            var task = new TaskWork
            {
                Id = Guid.NewGuid(),
                Name = taskDto.Name,
                Description = taskDto.Description,
                Status = taskDto.Status,
                SprintId = taskDto.SprintId,
                Images = taskDto.Images.Select(imgDto => new TaskImage { ImagePath = imgDto.ImagePath }).ToList(),
                UserId = Guid.Empty
            };

            await _taskRepository.AddAsync(task);

            return new TaskWorkDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId,
                Images = task.Images.Select(img => new TaskImageDTO { ImagePath = img.ImagePath }).ToList(),
                AssignedUserId = taskDto.AssignedUserId
            };
        }

        public async Task<TaskWorkDTO> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }
            return new TaskWorkDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId,
                Images = task.Images.Select(img => new TaskImageDTO { ImagePath = img.ImagePath }).ToList(),
                AssignedUserId  = task.UserId  
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
                SprintId = task.SprintId,
                Images = task.Images.Select(img => new TaskImageDTO { ImagePath = img.ImagePath }).ToList(),
                AssignedUserId = task.UserId
            });
        }

        public async Task UpdateAsync(TaskWorkDTO taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskDto.Id);
            if (task == null) return;

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
            task.UserId = taskDto.AssignedUserId;
            // Обновляем привязанного пользователя
            if (taskDto.AssignedUserId != null)
            {
                var user = await _userRepository.GetByIdAsync(taskDto.AssignedUserId);
                if (user != null)
                {
                    task.User = user;
                }
            }
            else
            {
                task.User = null; // Удаляем пользователя, если AssignedUserId = null
            }

            var imagesToRemove = task.Images.Where(img => !taskDto.Images.Any(dto => dto.ImagePath == img.ImagePath)).ToList();
            foreach (var image in imagesToRemove)
            {
                task.Images.Remove(image);
            }

            // Добавляем новые изображения к существующим
            foreach (var imgDto in taskDto.Images)
            {
                // Проверяем, чтобы избежать дублирования
                if (!task.Images.Any(img => img.ImagePath == imgDto.ImagePath))
                {
                    task.Images.Add(new TaskImage { ImagePath = imgDto.ImagePath });
                }
            }

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
        public async Task<UserDTO> GetUserAsync(Guid? id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    UserName = user.Email
                };
            }
            return null;
        }
       
    }
}

