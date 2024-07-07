using Application.DTO;

namespace Web.Models.ForCurrentUserModel
{
    public class CurrentUserSprintModel
    {
        public SprintDTO SprintDTOs { get; set; }
        public IEnumerable<TaskWorkDTO> TaskWorkDTOs { get; set; }
    }
}
