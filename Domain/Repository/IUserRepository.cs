﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByIdAsync(Guid? id);
        Task<User> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task RemoveSprintUserAsync(Guid sprintId);
    }
}
