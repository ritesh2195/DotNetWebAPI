using DemoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DemoWebAPI.Data;
using DemoWebAPI.Dtos;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private DbContext _dataContextEF;

        IUserRepository _userRepository;

        public UserController(IConfiguration config, IUserRepository userRepository)
        {
            _dataContextEF = new DbContext(config);

            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<User> GetUser()
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
