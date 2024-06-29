using Application.DTO;

namespace Web.Models.TaskWorkModel
{
    public class TaskImagesViewModel
    {
        public List<Guid> ImageIds { get; set; }
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public List<string>? ImagePaths { get; set; }
    }
}
