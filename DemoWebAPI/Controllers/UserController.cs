using DemoWebAPI.Dtos;
using DemoWebAPI.Models;
using DemoWebAPI.Repositories;
using DemoWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {

        private IUserRepository _userRepository;

        private IUserService _userService;

        public UserController(IConfiguration config, IUserRepository userRepository, IUserService userService)
        {

            _userRepository = userRepository;

            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userService.GetUsers();

            return users;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {

            User? user = _userService.GetUserById(id);

            try
            {
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            try
            {
                User user = _userService.CreateUser(userDto);

                return Ok(new
                {
                    message = "User created successfully",
                    userId = user.UserMasterId
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDto userDto)
        {
            try
            {
                _userService.UpdateUser(id, userDto);

                return StatusCode(201);
            }

            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    statusCode = 400,
                    errorMessage = ex.Message
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    errorMessage = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    errorMessage = ex.Message
                });
            }

        }
    }
}
