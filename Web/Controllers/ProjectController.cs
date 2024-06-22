using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ProjectModel;
using Web.Models.SprintModel;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ISprintService _sprintService;

        public ProjectController(IProjectService projectService, ISprintService sprintService)
        {
            _projectService = projectService;
            _sprintService = sprintService;
        }
                

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            var projectSprintDtos = new List<ProjectSprintDto>();

            foreach (var project in projects)
            {
                var sprints = await _sprintService.GetAllByProjectIdAsync(project.Id);
                projectSprintDtos.Add(new ProjectSprintDto
                {
                    Project = project,
                    Sprints = sprints
                });
            }

            return View(projectSprintDtos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                var projectDto = new ProjectDTO
                {
                    Name = projectViewModel.Name,
                    Description = projectViewModel.Description,
                    Sprints = new List<SprintDTO>()
                };

                await _projectService.AddAsync(projectDto);
                return RedirectToAction("Index", "Project");
            }
            return View(projectViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
            };

            return View(projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                var projectDto = new ProjectDTO
                {
                    Id = projectViewModel.Id,
                    Name = projectViewModel.Name,
                    Description = projectViewModel.Description,
                };

                await _projectService.UpdateAsync(projectDto);
                return RedirectToAction("Index", "Project");
            }
            return View(projectViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
            };

            return View(projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _projectService.DeleteAsync(id);
            return RedirectToAction("Index", "Project");
        }
    }

}
