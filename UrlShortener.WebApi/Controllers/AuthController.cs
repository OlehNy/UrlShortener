using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Core.Entities;
using UrlShortener.WebApi.Models.Auth;

namespace UrlShortener.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, 
            IConfiguration configuration,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }
    }
}
