using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ProjectModel;
using Web.Models.SprintModel;

namespace Web.Controllers
{
    public class SprintController : Controller
    {
        private readonly ISprintService _sprintService;
        private readonly ITaskWorkService _taskWorkService;

        public SprintController(ISprintService sprintService, ITaskWorkService taskWorkService)
        {
            _sprintService = sprintService;
            _taskWorkService = taskWorkService;
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

            var model = new SprintTaskDto
            {
                Sprint = sprint,
                Tasks = tasks.ToList()
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
            var model = new SprintViewModel
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Description = sprint.Description,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                ProjectId = sprint.ProjectId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SprintViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new SprintDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId
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
    }
}
