using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SprintDTO>? Sprints { get; set; }
    }
    public class CreatProjectDTO
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
