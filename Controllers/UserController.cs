using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HAdmin.Repositories;
using HAdmin.Models;
using HAdmin.Dtos;
using HAdmin.Security;

namespace HAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        public UserController(IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserOutput>>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            var userResult = users.Select(u => new UserOutput { Id = u.Id, Username = u.Username, IdNumber = u.IdNumber, PhoneNumber = u.PhoneNumber });
            return Ok(userResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserOutput>> GetUser(Guid id)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                return NotFound();

            UserOutput userOutput = new()
            {
                Id = user.Id,
                Username = user.Username,
                IdNumber = user.IdNumber,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(userOutput);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var hashedPassword = _hasher.Hash(createUserDto.Password);
            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                IdNumber = createUserDto.IdNumber,
                PhoneNumber = createUserDto.PhoneNumber,
                Password = hashedPassword
            };

            await _userRepository.Add(user);

            UserOutput userOutput = new()
            {
                Id = user.Id,
                Username = user.Username,
                IdNumber = user.IdNumber,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(userOutput);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            await _userRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            var userToUpdate = await _userRepository.Get(id);
            var isVerified = _hasher.Check(userToUpdate.Password, updateUserDto.OldPassword);
            if (!isVerified)
                return Unauthorized();

            var hashedPassword = _hasher.Hash(updateUserDto.NewPassword);
            User user = new()
            {
                Id = id,
                Username = userToUpdate.Username,
                IdNumber = userToUpdate.IdNumber,
                PhoneNumber = userToUpdate.PhoneNumber,
                Password = hashedPassword
            };

            UserOutput userOutput = new()
            {
                Id = id,
                Username = userToUpdate.Username,
                IdNumber = userToUpdate.IdNumber,
                PhoneNumber = userToUpdate.PhoneNumber
            };

            await _userRepository.Update(user);
            return Ok(userOutput);
        }
    }

}