using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagementAPI.EntityFramework.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TaskManagementDBEntites _dbContext;
        public LoginController(IConfiguration configuration, TaskManagementDBEntites dataContex)
        {
            _configuration = configuration;
            _dbContext = dataContex;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await Authenticate(userName, password, cancellationToken);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var token = Generate(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        private async Task<User> Authenticate(string userName, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password && 
                                              x.IsActive, cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string Generate(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                  _configuration["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddHours(3),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
