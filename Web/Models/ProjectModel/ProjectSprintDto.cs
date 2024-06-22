using Application.DTO;

namespace Web.Models.ProjectModel
{
    public class ProjectSprintDto
    {
        public ProjectDTO Project { get; set; }
        public IEnumerable<SprintDTO> Sprints { get; set; }
    }
}
