using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SprintDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public List<TaskWorkDTO>? Tasks { get; set; }
        public ICollection<UserDTO>? Users { get; set; }
        public List<Guid>? AssignedUserIds { get; set; }

    }

}
