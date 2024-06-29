using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskWork
    {
        public Guid Id { get; set; }
        public Guid SprintId { get; set; }
        public Sprint Sprint { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public ICollection<TaskImage>? Images { get; set; } = new List<TaskImage>();
        public ICollection<User> Users { get; set; }
    }
    public class TaskImage
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public Guid TaskWorkId { get; set; }
        public TaskWork TaskWork { get; set; }
    }
}
