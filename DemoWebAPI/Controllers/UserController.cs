using DemoWebAPI.Dtos;
using DemoWebAPI.Models;
using DemoWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {

        IUserRepository _userRepository;

        public UserController(IConfiguration config, IUserRepository userRepository)
        {

            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();

            return users;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {

            User? user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = "User ID is not valid or deleted"
                });
            }

            else
            {
                return Ok(user);
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "First Name is required."
                });
            }

            if (userDto.FirstName.Length > 50)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "First Name should not exceed 50 characters."
                });
            }

            if (string.IsNullOrWhiteSpace(userDto.LastName))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Last Name is required."
                });
            }

            if (userDto.LastName.Length > 50)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Last Name should not exceed 50 characters."
                });
            }

            if (string.IsNullOrWhiteSpace(userDto.Email))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Email is required."
                });
            }

            if (userDto.Email.Length > 100)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Email should not exceed 100 characters."
                });
            }

            if (Regex.IsMatch(userDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Email is not valid."
                });
            }

            if (string.IsNullOrEmpty(userDto.Gender))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Gender is required."
                });
            }

            User userDb = new User();

            userDb.FirstName = userDto.FirstName;
            userDb.LastName = userDto.LastName;
            userDb.Email = userDto.Email;
            userDb.Gender = userDto.Gender;
            userDb.IsActive = userDto.IsActive;
            userDb.IsDeleted = false;

            _userRepository.AddEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to create the user");

        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            User? userDb = _userRepository.GetUserById(id);

            if (userDb == null)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = "User ID is not valid or deleted"
                });
            }

            else if (userDb != null)
            {
                userDb.FirstName = user.FirstName;

                userDb.LastName = user.LastName;

                userDb.Email = user.Email;

                userDb.Gender = user.Gender;

                userDb.IsActive = user.IsActive;

                userDb.IsDeleted = false;

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to update the user");
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userDb = _userRepository.GetUserById(id);

            if (userDb == null)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = "User ID is not valid or deleted"
                });
            }

            userDb.IsDeleted = true;

            if (_userRepository.SaveChanges())
            {
                return Ok(new
                {
                    statusCode = 200,
                    message = "User deleted successfully"
                });
            }

            return StatusCode(500, new
            {
                statusCode = 500,
                errorMessage = "Failed to delete the user"
            });
        }

    }
}
