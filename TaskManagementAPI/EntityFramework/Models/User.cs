using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.EntityFramework.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
    }
}
