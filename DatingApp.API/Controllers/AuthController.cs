using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRespository _Repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRespository Repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _Repo = Repo;

        }
        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //Validate Request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _Repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdUser = await _Repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetaileddto>(createdUser);

            return CreatedAtRoute("GetUser", new {controller = "Users", id = createdUser.Id},userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           
                
            var userFromRepo = await _Repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            var token = tokenhandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListFto>(userFromRepo);

            return Ok(new {
                 token = tokenhandler.WriteToken(token),user
            });
                    
        }
    }
}