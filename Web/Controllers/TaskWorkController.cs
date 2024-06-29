using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models.TaskWorkModel;

namespace Web.Controllers
{
    public class TaskWorkController : Controller
    {
        private readonly ITaskWorkService _taskService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaskWorkController(ITaskWorkService taskService, IWebHostEnvironment webHostEnvironment)
        {
            _taskService = taskService;
            _webHostEnvironment = webHostEnvironment;
        }

        
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
                SprintId = task.SprintId,
                ImagePaths = task.Images.Select(img => img.ImagePath).ToList()
            };

            return View(model);
        }
        public async Task<IActionResult> ViewImages(Guid taskId)
        {
            var task = await _taskService.GetByIdAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskImagesViewModel
            {
                TaskId = task.Id,
                ImagePaths = task.Images.Select(img => img.ImagePath).ToList(),
                Name = task.Name,
                ImageIds = task.Images.Select(img => img.Id).ToList()
            };

            return View(model);
        }
        
        [HttpGet]
        public IActionResult CreateTask(Guid sprintId)
        {
            var model = new TaskViewModel { SprintId = sprintId };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskViewModel model, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                var imagePaths = await SaveImagesAsync(Images);

                var dto = new TaskWorkDTO
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    SprintId = model.SprintId,
                    Images = imagePaths.Select(path => new TaskImageDTO { ImagePath = path }).ToList()
                };

                await _taskService.CreateAsync(dto);
                return RedirectToAction("Details", "Sprint", new { id = model.SprintId });
            }
            return View(model);
        }

        
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
                SprintId = task.SprintId,
                ImagePaths = task.Images.Select(img => img.ImagePath).ToList()
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskWork(TaskViewModel model, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                var task = await _taskService.GetByIdAsync(model.Id);
                if (task == null)
                {
                    return NotFound();
                }

                var newImagePaths = await SaveImagesAsync(Images);

                var allImageDtos = task.Images
                    .Concat(newImagePaths.Select(path => new TaskImageDTO { ImagePath = path }))
                    .ToList();

                var taskDto = new TaskWorkDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    SprintId = model.SprintId,
                    Images = allImageDtos
                };

                await _taskService.UpdateAsync(taskDto);
                return RedirectToAction("TaskDetails", "TaskWork", new { id = model.Id}); 
            }
            return View(model);
        }        
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
        private async Task<List<string>> SaveImagesAsync(List<IFormFile> images)
        {
            var imagePaths = new List<string>();
            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    imagePaths.Add(image.FileName); 
                }
            }
            return imagePaths;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImages(Guid taskId, List<string> ImageIds)
        {
            if (taskId == Guid.Empty)
            {
                return BadRequest("Task Id is required.");
            }        

            try
            {
                var task = await _taskService.GetByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                // Удаление изображений по их идентификаторам
                foreach (var imageIdString in ImageIds)
                {
                    if (Guid.TryParse(imageIdString, out Guid imageId))
                    {
                        var imageToRemove = task.Images.FirstOrDefault(img => img.Id == imageId);
                        if (imageToRemove != null)
                        {
                            task.Images.Remove(imageToRemove);
                        }
                    }
                    else
                    {
                        // Handle invalid Guid format if necessary
                        // For example, log an error or skip this item
                    }
                }

                    await _taskService.UpdateAsync(task); // Обновление задачи в репозитории
                
                var Imagetask = await _taskService.GetByIdAsync(taskId);
                var qwe = Imagetask;
                return RedirectToAction("TaskDetails","TaskWork", new { id = task.Id }); // Перенаправление на страницу с изображениями задачи
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

