using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TaskWork> Tasks { get; set; }
        public DbSet<SprintUser> SprintUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SprintUser>()
                .HasKey(su => new { su.SprintId, su.UserId });

            modelBuilder.Entity<SprintUser>()
                .HasOne(su => su.Sprint)
                .WithMany(s => s.SprintUsers)
                .HasForeignKey(su => su.SprintId);

            modelBuilder.Entity<SprintUser>()
                .HasOne(su => su.User)
                .WithMany(u => u.SprintUsers)
                .HasForeignKey(su => su.UserId);

            modelBuilder.Entity<TaskWork>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
