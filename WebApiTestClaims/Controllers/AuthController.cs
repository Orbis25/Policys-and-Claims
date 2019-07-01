using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApiTestClaims.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDb _db;
        public AuthController(IConfiguration configuration , ApplicationDb db)
        {
            _configuration = configuration;
            var d = User;
            _db = db;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]User model)
        {
            return GetToken(model);
        }

        [HttpGet("claims")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "admin,otro")]
        public string Claims()
        {
            return "orbi";
        }

        [HttpGet("claims2")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "otro")]
        public string Claims2()
        {
            return "orbi2";
        }

        private IActionResult GetToken(User model)
        {

            var roles = _db.Users.Include(x => x.Roles).First(x => x.UserName == model.UserName);
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName)
            };

            foreach (var rol in roles.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret_Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token =  new JwtSecurityToken(
               issuer: "devteams.com",
               audience: "devteams.com",
               claims: claims,
               signingCredentials: creds
               );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}