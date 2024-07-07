using Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.TaskWorkModel
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }
        public Guid SprintId { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<string>? ImagePaths { get; set; } = new List<string>();
        public string? AssignedUserName { get; set; }
        public Guid? AssignedUserId { get; set; } // Поле для хранения идентификатора назначенного пользователя
        public List<UserDTO> SprintUsers { get; set; } = new List<UserDTO> ();
    }
}
