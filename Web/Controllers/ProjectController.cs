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

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            var projectViewModels = projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Sprints = project.Sprints.Select(sprint => new SprintViewModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate, 
                    ProjectId = sprint.ProjectId
                }).ToList() ?? new List<SprintViewModel>()
            }).ToList();

            return View(projectViewModels);
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
                    Sprints = new List<SprintDto>()
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
                Sprints = project.Sprints.Select(sprint => new SprintViewModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    ProjectId = sprint.ProjectId
                }).ToList() ?? new List<SprintViewModel>()
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
                    Sprints = projectViewModel.Sprints.Select(sprint => new SprintDto
                    {
                        Id = sprint.Id,
                        Name = sprint.Name,
                        StartDate = sprint.StartDate,
                        EndDate = sprint.EndDate,
                        ProjectId = sprint.ProjectId
                    }).ToList()
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
                Sprints = project.Sprints.Select(sprint => new SprintViewModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    ProjectId = sprint.ProjectId
                }).ToList()
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
