using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.ProjectModel;
using Web.Models.SprintModel;

namespace Web.Controllers
{
    public class SprintController : Controller
    {
        private readonly ISprintService _sprintService;
        private readonly ITaskWorkService _taskWorkService;
        private readonly IAuthorisationService _authorisationService;

        public SprintController(ISprintService sprintService, ITaskWorkService taskWorkService, IAuthorisationService authorisationService)
        {
            _sprintService = sprintService;
            _taskWorkService = taskWorkService;
            _authorisationService = authorisationService;
        }

        public IActionResult Create(Guid projectId)
        {
            var model = new SprintViewModel { ProjectId = projectId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SprintViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new SprintDTO
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId
                };

                await _sprintService.CreateAsync(dto);
                return RedirectToAction("Index", "Project", new { projectId = model.ProjectId });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var sprint = await _sprintService.GetByIdAsync(Id);
            
            if (sprint == null)
            {
                return NotFound();
            }
            var tasks = await _taskWorkService.GetAllBySprintIdAsync(sprint.Id);
            var users = await _sprintService.GetAllUsersBySprintIdAsync(sprint.Id); // Используем новый метод

            var model = new SprintTaskDto
            {
                Sprint = sprint,
                Tasks = tasks.ToList(),
                Users = users.ToList() // Добавляем пользователей в модель
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
           
            var sprint = await _sprintService.GetByIdAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }

            var assignedUserIds = sprint.AssignedUserIds;
            var allUsers = await _sprintService.GetAllUsersAsync();

            var assignedUsers = allUsers
                .Where(u => assignedUserIds.Contains(u.Id))
                .ToList();

            var model = new SprintViewModel
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                Name = sprint.Name,
                Description = sprint.Description,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Users = assignedUsers, // Здесь только добавленные пользователи
                AssignedUserIds = assignedUserIds
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SprintViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sprint = await _sprintService.GetByIdAsync(model.Id);
                if (sprint == null)
                {
                    return NotFound();
                }

                var assignedUserIds = sprint.AssignedUserIds;
                var allUsers = await _sprintService.GetAllUsersAsync();

                var assignedUsers = allUsers
                    .Where(u => assignedUserIds.Contains(u.Id))
                    .ToList();

                var dto = new SprintDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId,
                    Users = assignedUsers, // Здесь только добавленные пользователи
                    AssignedUserIds = assignedUserIds

                };

                await _sprintService.UpdateAsync(dto);
                return RedirectToAction("Details", "Sprint", new { Id = model.Id });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(SprintTaskDto model)
        {
            var sprint = await _sprintService.GetByIdAsync(model.Sprint.Id);
            if (sprint != null)
            {
                await _sprintService.DeleteAsync(model.Sprint.Id);
                return RedirectToAction("Index", "Project", new { projectId = sprint.ProjectId });
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SelectUsers(Guid sprintId)
        {
            var sprint = await _sprintService.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                return NotFound();
            }

            var users = await _sprintService.GetAllUsersAsync();
            var assignedUserIds = await _sprintService.GetAssignedUserIdsAsync(sprintId);

            var model = new SelectUsersViewModel
            {
                SprintId = sprintId,
                Users = users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsAssigned = assignedUserIds.Contains(u.Id)
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AssignUsers(Guid sprintId, List<Guid> userIds)
        {
            var currentUsers = await _sprintService.GetAssignedUserIdsAsync(sprintId);

            var usersToAdd = userIds.Except(currentUsers).ToList();
            var usersToRemove = currentUsers.Except(userIds).ToList();

            foreach (var userId in usersToAdd)
            {
                await _sprintService.AssignUserToSprint(sprintId, userId);
            }

            foreach (var userId in usersToRemove)
            {
                await _sprintService.RemoveUserFromSprint(sprintId, userId);
            }

            return RedirectToAction("Edit", new { id = sprintId });
        }
    }
}
