using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepositary _repo;
        private readonly IMapper _mapper;
    

        public UsersController(IDatingRepositary Repo, IMapper mapper)
        {
            this._mapper = mapper;
            _repo = Repo;
        }

        [HttpGet]
        public async Task<IActionResult> Getsuers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListFto>>(users);

            return Ok(usersToReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetaileddto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userforUpdateDto)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
               return Unauthorized();

                var userFromRepo = await _repo.GetUser(id);

                _mapper.Map(userforUpdateDto, userFromRepo);

                if(await _repo.SaveAll())
                   return NoContent();

                throw new Exception($"Updating user{id} failed on save");   
            
        }
    }
}