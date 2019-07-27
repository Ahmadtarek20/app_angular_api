using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zwajApp.API.Data;
using zwajApp.API.Dtos;
using zwajApp.API.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace zwajApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _rebo;
        private readonly IConfiguration _confg;

        public AuthController(IAuthRepository rebo, IConfiguration confg)
        {
            _confg = confg;
            _rebo = rebo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRigesterDto userForRigesterDto)
        {
            //validation
            userForRigesterDto.Username = userForRigesterDto.Username.ToLower();
            if (await _rebo.UserExists(userForRigesterDto.Username))
                return BadRequest("هذا المشترك موجود");
            var usertocreat = new Users
            {
                UserName = userForRigesterDto.Username
            };
            var CreatedUser = await _rebo.Register(usertocreat, userForRigesterDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userfromrebo = await _rebo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);
            if (userfromrebo == null) return Unauthorized();
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userfromrebo.id.ToString()),
                new Claim(ClaimTypes.Name,userfromrebo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_confg.GetSection("AppSettings:Token").Value));
            var creds =new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
            var tokenDiscrbtor =new SecurityTokenDescriptor{
                Subject =new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDiscrbtor);  
            return Ok(new{
                token=tokenhandler.WriteToken(token)
            });
        }

    }
}