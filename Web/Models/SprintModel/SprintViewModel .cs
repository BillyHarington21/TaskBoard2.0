using Application.DTO;
using Web.Models.TaskWorkModel;

namespace Web.Models.SprintModel
{
    public class SprintViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public List<TaskViewModel>? Tasks { get; set; }
        public List<UserDTO>? Users { get; set; } 
        public List<Guid>? AssignedUserIds { get; set; } 
    }
}
