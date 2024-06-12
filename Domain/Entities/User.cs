﻿namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsBlocked { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
