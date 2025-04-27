using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.EntityFramework.Models;
using Yrefy.UnitTest.Helper;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementTest.ControllerTest
{
    public class LoginControllerTests
    {
        private readonly IConfiguration _configuration;
        private readonly Mock<TaskManagementDBEntites> _mockDbContext;
        private readonly LoginController _loginController;

        public LoginControllerTests()
        {
            _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            _mockDbContext = new Mock<TaskManagementDBEntites>();
            _loginController = new LoginController(_configuration, _mockDbContext.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var userName = "validUser";
            var password = "validPassword";
            var cancellationToken = CancellationToken.None;

            var user = new User { UserName = "validUser", Password = "validPassword", IsActive = true, Email = "user@example.com", Role = "Admin" };

            var data = new List<User> { user }.AsQueryable();

            _mockDbContext.Setup(db => db.Users).Returns(MockingDbSet.GetQueryableMockDbSet(data).Object);

            // Act
            var result = await _loginController.Login(userName, password, cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("Token", okResult.Value.ToString());
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            var userName = "invalidUser";
            var password = "somePassword";
            var cancellationToken = CancellationToken.None;

            var data = new List<User>().AsQueryable();
            _mockDbContext.Setup(db => db.Users).Returns(MockingDbSet.GetQueryableMockDbSet(data).Object);

            // Act
            var result = await _loginController.Login(userName, password, cancellationToken);
            var unauthorizedResult = result as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var userName = "validUser";
            var password = "wrongPassword";
            var cancellationToken = CancellationToken.None;

            var user = new User { UserName = "validUser", Password = "validPassword", IsActive = true, Email = "user@example.com", Role = "Admin" };

            var data = new List<User> { user }.AsQueryable();
            _mockDbContext.Setup(db => db.Users).Returns(MockingDbSet.GetQueryableMockDbSet(data).Object);

            // Act
            var result = await _loginController.Login(userName, password, cancellationToken);
            var unauthorizedResult = result as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WithInactiveUser_ReturnsUnauthorized()
        {
            // Arrange
            var userName = "inactiveUser";
            var password = "validPassword";
            var cancellationToken = CancellationToken.None;

            var user = new User { UserName = "inactiveUser", Password = "validPassword", IsActive = false, Email = "inactive@example.com", Role = "User" };

            var data = new List<User> { user }.AsQueryable();
            _mockDbContext.Setup(db => db.Users).Returns(MockingDbSet.GetQueryableMockDbSet(data).Object);

            // Act
            var result = await _loginController.Login(userName, password, cancellationToken);
            var unauthorizedResult = result as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken_WhenUserIsActive()
        {
            // Arrange
            var userName = "validActiveUser";
            var password = "correctPassword";
            var cancellationToken = CancellationToken.None;

            var user = new User { UserName = "validActiveUser", Password = "correctPassword", IsActive = true, Email = "activeuser@example.com", Role = "Admin" };

            var data = new List<User> { user }.AsQueryable();
            _mockDbContext.Setup(db => db.Users).Returns(MockingDbSet.GetQueryableMockDbSet(data).Object);

            // Act
            var result = await _loginController.Login(userName, password, cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("Token", okResult.Value.ToString());
        }
    }
}
