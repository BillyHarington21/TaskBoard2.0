using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.TaskWorkModel;

namespace Web.Controllers
{
    public class TaskWorkController : Controller
    {
        private readonly ITaskWorkService _taskService;
        

        public TaskWorkController(ITaskWorkService taskService)
        {
            _taskService = taskService;
        }

        // GET: Task/Details/5
        public async Task<IActionResult> TaskDetails(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId
            };

            return View(model);
        }

        // GET: Task/Create
        [HttpGet]
        public IActionResult CreateTask(Guid sprintId)
        {
            var model = new TaskViewModel { SprintId = sprintId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new TaskWorkDTO
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    SprintId = model.SprintId
                };

                await _taskService.CreateAsync(dto);
                return RedirectToAction("Details", "Sprint", new { id = model.SprintId });
            }
            return View(model);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> EditTaskWork(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                SprintId = task.SprintId
            };

            return View(model);
        }

        // POST: Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskWork(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taskDto = new TaskWorkDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    SprintId = model.SprintId
                };

                await _taskService.UpdateAsync(taskDto);
                return RedirectToAction("Details", "Sprint", new { id = model.SprintId });
            }
            return View(model);
        }

        // POST: Task/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task != null)
            {
                await _taskService.DeleteAsync(id);
                return RedirectToAction("Details", "Sprint", new { id = task.SprintId });
            }
            return RedirectToAction("TaskDetails", "TaskWork");
        }
    }
}

