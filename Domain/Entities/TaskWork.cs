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
        public ICollection<User> Users { get; set; }
    }
}
