using Application.DTO;

namespace Web.Models.SprintModel
{
    public class SprintTaskDto
    {
        public SprintDTO Sprint { get; set; }
        public List<TaskWorkDTO> Tasks { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
