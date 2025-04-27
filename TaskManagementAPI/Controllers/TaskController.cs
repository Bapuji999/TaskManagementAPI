using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.CommandModel;
using TaskManagementAPI.EntityFramework.Models;
using TaskManagementAPI.QueryModel;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagementDBEntites _dbContext;

        public TaskController(TaskManagementDBEntites dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="addTaskCommandModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpPost("CreateNewTask")]
        public async Task<IActionResult> CreateNewTask([FromBody] AddTaskCommandModel addTaskCommandModel, CancellationToken cancellationToken)
        {
            if (addTaskCommandModel == null)
                return BadRequest("Task data is required.");

            try
            {
                var newTask = new EntityFramework.Models.Task
                {
                    Title = addTaskCommandModel.Title,
                    Description = addTaskCommandModel.Description,
                    DueDate = addTaskCommandModel.DueDate
                };

                await _dbContext.Task.AddAsync(newTask, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Ok(new { Message = "Task created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get task details by ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager,User")]
        [HttpGet("GetTaskDetailsById")]
        public async Task<IActionResult> GetTaskDetailsById(int taskId, CancellationToken cancellationToken)
        {
            return await GetTaskDetailsAsync(taskId, cancellationToken);
        }

        /// <summary>
        /// Get tasks assigned to a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpGet("GetTaskDetailsByUserId")]
        public async Task<IActionResult> GetTaskDetailsByUserId(int userId, CancellationToken cancellationToken)
        {
            return await GetTaskDetailsAsync(userId, cancellationToken, byUserId: true);
        }

        private async Task<IActionResult> GetTaskDetailsAsync(int identifier, CancellationToken cancellationToken, bool byUserId = false)
        {
            try
            {
                var query = _dbContext.Task
                    .Include(x => x.AssignedUser)
                    .Include(x => x.TaskComments)
                    .AsQueryable();

                if (byUserId)
                    query = query.Where(x => x.AssignedUserId == identifier);
                else
                    query = query.Where(x => x.Id == identifier);

                var existingTask = await query.FirstOrDefaultAsync(cancellationToken);

                if (existingTask == null)
                    return NotFound("No task record found for the given ID.");

                var taskDetails = MapTaskToTaskDetailQueryModel(existingTask);

                return Ok(taskDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        private TaskDetailQueryModel MapTaskToTaskDetailQueryModel(EntityFramework.Models.Task existingTask)
        {
            return new TaskDetailQueryModel
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                AssignedUserId = existingTask.AssignedUserId,
                AssignedUserName = existingTask.AssignedUser?.UserName,
                Status = existingTask.Status,
                DueDate = existingTask.DueDate,
                CreatedAt = existingTask.CreatedAt,
                TaskComments = existingTask.TaskComments?.Select(comment => new TaskCommentsQueryModel
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    TaskId = comment.TaskId,
                    Comment = comment.Comment,
                }).ToList()
            };
        }
    }
}