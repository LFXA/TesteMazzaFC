using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteMazzaAPI.Models;

namespace TesteMazzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       [HttpPost("login")]
        public IActionResult Index([FromForm]Login login)
        {
            return Ok(new { token = login.Email });
        }
    }
}
