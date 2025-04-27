using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.CommandModel
{
    public class AddTaskCommandModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
