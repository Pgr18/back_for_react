using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using My_WebApp.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using My_WebApp.DbContexts;
using My_WebApp.Utils;
using Microsoft.EntityFrameworkCore;
using My_WebApp.Models.DTO;


namespace My_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private List<User> _users = new List<User>
        {
            new User { Login = "admin@mail.ru", Password = "12345", Role = "admin" }
        };
        //    new User { Login = "user@mail.ru", Password = "123456", Role = "user" }
        //};

        private ApplicationContext _context;

        public AuthController()
        {
            _context = new ApplicationContext();
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] RegisterRequestDto registerRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == registerRequest.Login);
            if (user != null)
            {
                return BadRequest("User with this login already exists");
            }
            _context.Users.Add(new User
            {
                Login = registerRequest.Login,
                Password = AuthUtils.HashPassword(registerRequest.Password),
                Role = "user"
            });
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("/login")]

        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
           var identity = GetIdentity(loginRequest.Login,loginRequest.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password!" });
            }
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore:now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                usename = identity.Name,
            };

            return Ok(JsonConvert.SerializeObject(response));
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            

            var user = _context.Users.FirstOrDefault(u => u.Login == username);
            if (user == null || !AuthUtils.VerifyPassword(password, user.Password))
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }

    }
}
