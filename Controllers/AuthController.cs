using BukiApi.Data;
using BukiApi.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BukiApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserLoginDTO> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<UserLoginDTO> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO) 
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new 
                    JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else {
                return Unauthorized(new { message = "E-mail ou usuário incorreto!"});
            }
        }
    }
}
