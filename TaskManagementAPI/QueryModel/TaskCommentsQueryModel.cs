namespace TaskManagementAPI.QueryModel
{
    public class TaskCommentsQueryModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
