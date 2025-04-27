namespace TaskManagementAPI.EntityFramework.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
