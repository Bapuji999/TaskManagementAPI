using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.EntityFramework.Models
{
    public class TaskManagementDBEntites : DbContext
    {
        public TaskManagementDBEntites()
        {
        }

        public TaskManagementDBEntites(DbContextOptions<TaskManagementDBEntites> options)
            : base(options)
        {
        }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskComment> TaskComment { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "john.doe", Email = "john.doe@example.com", Role = "Manager", Password = "Mabc@123", IsActive = true },
                new User { Id = 2, UserName = "jane.smith", Email = "jane.smith@example.com", Role = "User", Password = "Abc@123",IsActive = true }
            );

            // Seeding Tasks
            modelBuilder.Entity<Task>().HasData(
                new Task { Id = 1, Title = "Initial Setup", DueDate = DateTime.UtcNow.AddDays(1), Description = "Setup project structure", AssignedUserId = 1 },
                new Task { Id = 2, Title = "Database Migration", DueDate = DateTime.UtcNow.AddDays(1), Description = "Create database and apply migrations", AssignedUserId = 2 }
            );

            // Seeding Task Comments
            modelBuilder.Entity<TaskComment>().HasData(
                new TaskComment { Id = 1, TaskId = 1, Comment = "Started working on project setup", UserId = 1 },
                new TaskComment { Id = 2, TaskId = 2, Comment = "Database created successfully", UserId = 2 }
            );
        }
    }
}
