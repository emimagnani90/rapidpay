using DataAccess.Repositories.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using RapidPay.DTO;
using RapidPay.ConfigurationSections;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataAccess.UnitOfWork.Interfaces;

namespace RapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public UserController(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Complete user and password");
            }

            var existingUser = _unitOfWork.User.GetByUserName(user.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already in use");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _unitOfWork.User.AddUser(user);
            _unitOfWork.Complete();
            return Ok(user);
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Complete user and password");
            }
            var user = this._unitOfWork.User.GetByUserName(request.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Invalid user or password");
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Key),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("UserName", user.UserName),
                        new Claim("LastName", user.LastName),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
