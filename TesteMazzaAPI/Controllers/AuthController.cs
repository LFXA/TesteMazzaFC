using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TesteMazzaAPI.Data;
using TesteMazzaAPI.Models;

namespace TesteMazzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TesteMazzaContext _context;
        public AuthController(TesteMazzaContext context)
        {
            _context = context;
        }
       [HttpPost("login")]
        public IActionResult Index([FromForm]Login login)
        {
            var user = _context.Users.Where(t => t.Email == login.Email && t.Password == login.Password).FirstOrDefault();
            if(user == null)
            {
                return Unauthorized();
            }
            return Ok(new { token = login.Email });
        }

        private JwtSecurityToken GenerateToken()
        {
            var jwtSecurityToken = new JwtSecurityToken();

            return jwtSecurityToken;
        }
    }


}
