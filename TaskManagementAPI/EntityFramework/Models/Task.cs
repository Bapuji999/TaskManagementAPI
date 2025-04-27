using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.EntityFramework.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssignedUserId { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User AssignedUser { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
    }
}
