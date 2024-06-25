﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class TaskWorkDTO
    {
        public Guid Id { get; set; }
        public Guid SprintId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

    }
}