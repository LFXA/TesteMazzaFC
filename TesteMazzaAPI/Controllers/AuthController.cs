using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TesteMazzaAPI.Data;
using TesteMazzaAPI.Models;

namespace TesteMazzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TesteMazzaContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(TesteMazzaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [HttpPost("login")]
        public IActionResult Index([FromForm]Login login)
        {
            var user = _context.Users.Where(t => t.Email == login.Email && t.Password == login.Password).FirstOrDefault();
            if(user == null)
            {
                return Unauthorized();
            }
            JwtSecurityToken jwtSecurityToken = GenerateToken(login);
            var JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(new {token = JwtToken, email = login.Email });
        }

        private JwtSecurityToken GenerateToken(Login user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


            };
            var symmetricSecurrityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurrityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials                
                );
            
            return jwtSecurityToken;
        }
    }


}
