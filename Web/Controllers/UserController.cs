using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ForCurrentUserModel;
using Web.Models.ProjectModel;
using Web.Models.SprintModel;

namespace Web.Controllers
{
    public class UserController : Controller
    {
       private readonly ISprintService _sprintService;
       private readonly ITaskWorkService _taskWorkService;
       public UserController(ISprintService sprintService,ITaskWorkService taskWorkService)
       {
            _sprintService = sprintService;
            _taskWorkService = taskWorkService;
       }
       public async Task<IActionResult> MySprints()
       {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Преобразование строки идентификатора в тип Guid
            Guid userId;
            try
            {
                userId = Guid.Parse(userIdString);
            }
            catch (FormatException)
            {
                // Обработка ошибки, если строка не может быть преобразована в Guid
                return BadRequest("Invalid user ID format.");
            }               
                      
            var sprints = await _sprintService.GetSprintsByUserIdAsync(userId);
            var CurrentUserSprints = new List<CurrentUserSprintModel>();

            foreach (var sprint in sprints)
            {
                var tasks = await _taskWorkService.GetAllBySprintIdAsync(sprint.Id);
                var tasksOfUser = tasks.Where(task => task.AssignedUserId == userId).ToList();
                CurrentUserSprints.Add(new CurrentUserSprintModel
                {
                   SprintDTOs = sprint,
                   TaskWorkDTOs = tasksOfUser
                });
            }

            return View(CurrentUserSprints);
       }
    }
}
