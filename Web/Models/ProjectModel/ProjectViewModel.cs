using Web.Models.SprintModel;

namespace Web.Models.ProjectModel
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SprintViewModel>? Sprints { get; set; }
    }
}
