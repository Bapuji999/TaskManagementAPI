namespace TaskManagementAPI.QueryModel
{
    public class TaskDetailQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TaskCommentsQueryModel> TaskComments { get; set; }
    }
}
