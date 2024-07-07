using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<User>? User { get; set; }
        public ICollection<TaskWork>? Tasks { get; set; }
        public ICollection<SprintUser> SprintUsers { get; set; } = new List<SprintUser>();
    }
    public class SprintUser
    {
        public Guid SprintId { get; set; }
        public Sprint Sprint { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
