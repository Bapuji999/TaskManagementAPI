using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementAPI.CommandModel;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.EntityFramework.Models;
using TaskManagementAPI.QueryModel;
using Yrefy.UnitTest.Helper;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementTest.ControllerTest
{
    public class TaskControllerTests
    {
        private readonly Mock<TaskManagementDBEntites> _mockDbContext;
        private readonly Mock<ILogger<TaskController>> _mockLogger;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockDbContext = new Mock<TaskManagementDBEntites>();
            _mockLogger = new Mock<ILogger<TaskController>>();
            _controller = new TaskController(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateNewTask_ValidData_ReturnsOkResult()
        {
            // Arrange
            var commandModel = new AddTaskCommandModel
            {
                Title = "Test Task",
                Description = "Test Task Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            var mockDbSet = new Mock<DbSet<TaskManagementAPI.EntityFramework.Models.Task>>();
            _mockDbContext.Setup(x => x.Task).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.CreateNewTask(commandModel, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Task created successfully.", okResult.Value.ToString());
        }

        [Fact]
        public async Task CreateNewTask_NullCommandModel_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateNewTask(null, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task data is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetTaskDetailsById_ValidTaskId_ReturnsOkResult()
        {
            // Arrange
            var taskId = 1;
            var mockTask = new List<TaskManagementAPI.EntityFramework.Models.Task>{
                new()
                {

                    Id = taskId,
                    Title = "Test Task",
                    Description = "Test Task Description",
                    DueDate = DateTime.Now.AddDays(1)
                }
            }.AsQueryable();

            _mockDbContext.Setup(x => x.Task).Returns(MockingDbSet.GetQueryableMockDbSet(mockTask).Object);

            // Act
            var result = await _controller.GetTaskDetailsById(taskId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var taskDetail = Assert.IsType<TaskDetailQueryModel>(okResult.Value);
            Assert.Equal("Test Task", taskDetail.Title);
        }

        [Fact]
        public async Task GetTaskDetailsById_TaskNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var taskId = 999;

            _mockDbContext.Setup(x => x.Task)
                .Returns(MockingDbSet.GetQueryableMockDbSet(new List<TaskManagementAPI.EntityFramework.Models.Task>().AsQueryable()).Object);

            // Act
            var result = await _controller.GetTaskDetailsById(taskId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No task record found for the given ID.", notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task GetTaskDetailsByUserId_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var mockTask = new List<TaskManagementAPI.EntityFramework.Models.Task>
            {
                new()
                {
                    Id = 1,
                    Title = "Test Task",
                    Description = "Test Task Description",
                    AssignedUserId = userId,
                    DueDate = DateTime.Now.AddDays(1)
                }
            }.AsQueryable();

            _mockDbContext.Setup(x => x.Task)
                .Returns(MockingDbSet.GetQueryableMockDbSet(mockTask).Object);

            // Act
            var result = await _controller.GetTaskDetailsByUserId(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var taskDetail = Assert.IsType<TaskDetailQueryModel>(okResult.Value);
            Assert.Equal("Test Task", taskDetail.Title);
        }

        [Fact]
        public async Task GetTaskDetailsByUserId_UserHasNoTasks_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = 999;

            _mockDbContext.Setup(x => x.Task)
                .Returns(MockingDbSet.GetQueryableMockDbSet(new List<TaskManagementAPI.EntityFramework.Models.Task>().AsQueryable()).Object);

            // Act
            var result = await _controller.GetTaskDetailsByUserId(userId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No task record found for the given ID.", notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task GetTaskDetailsById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var taskId = 1;
            _mockDbContext.Setup(x => x.Task).Throws(new Exception("Database error"));

            // Act
            var result = await _controller.GetTaskDetailsById(taskId, CancellationToken.None);

            // Assert
            var objectResultResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResultResult.StatusCode);
        }

        [Fact]
        public async Task CreateNewTask_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var commandModel = new AddTaskCommandModel
            {
                Title = "Test Task",
                Description = "Test Task Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            var mockDbSet = new Mock<DbSet<TaskManagementAPI.EntityFramework.Models.Task>>();
            _mockDbContext.Setup(x => x.Task).Returns(mockDbSet.Object);
            _mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateNewTask(commandModel, CancellationToken.None);

            // Assert
            var objectResultResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResultResult.StatusCode);
        }

        [Fact]
        public async Task CreateNewTask_InvalidData_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateNewTask(null, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task data is required.", badRequestResult.Value);
        }
    }
}
