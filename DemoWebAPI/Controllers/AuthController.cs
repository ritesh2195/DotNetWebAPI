using DemoWebAPI.Data;
using DemoWebAPI.Dtos;
using DemoWebAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        private IJwtTokenService _jwtTokenService;

        private AppDbContext _dataContextEF;
        public AuthController(IConfiguration config, IJwtTokenService jwtTokenService)
        {
            _config = config;

            _jwtTokenService = jwtTokenService;

            _dataContextEF = new AppDbContext(config);
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(UserRegistrationDto registrationDto)
        {
            if (registrationDto == null)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "request payload can't be null"
                });
            }

            if (string.IsNullOrEmpty(registrationDto.Password))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Password can't be empty"
                });
            }

            if (registrationDto.Password != registrationDto.ConfirmPassword)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "Password and Confirm Password do not match"
                });
            }

            AuthMaster? auth = _dataContextEF.AuthMasters
                .Where(x => x.Email == registrationDto.Email)
                .FirstOrDefault();

            if (auth != null)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errorMessage = "A user with this email already exists"
                });
            }

            byte[] passwordSalt = new byte[128 / 8];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            string passwordKeyPlusSalt = _config.GetSection("AppSettings:PasswordKey").Value
                + Convert.ToBase64String(passwordSalt);

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: registrationDto.Password,
                salt: Encoding.ASCII.GetBytes(passwordKeyPlusSalt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            AuthMaster authDb = new AuthMaster();

            authDb.Email = registrationDto.Email;

            authDb.PasswordHash = passwordHash;

            authDb.PasswordSalt = passwordSalt;

            _dataContextEF.Add(authDb);

            if (_dataContextEF.SaveChanges() > 0)
            {
                return Ok("User registered successfully");
            }

            return StatusCode(500, "An error occurred while registering the user.");

        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto loginDto)
        {
            AuthMaster? auth = _dataContextEF.AuthMasters
                .Where(x => x.Email == loginDto.Email)
                .FirstOrDefault();

            if (auth == null)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    errorMessage = "User with this email id doesn't exist"
                });
            }

            string passwordKeyPlusSalt = _config.GetSection("AppSettings:PasswordKey").Value
                + Convert.ToBase64String(auth.PasswordSalt);

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: loginDto.Password,
                salt: Encoding.ASCII.GetBytes(passwordKeyPlusSalt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            if (passwordHash.SequenceEqual(auth.PasswordHash))
            {
                string jwtToken = _jwtTokenService.GenerateToken(auth.Email);

                return Ok(new
                {
                    message = "Login successful",
                    token = jwtToken,
                });
            }
            else
            {
                return Unauthorized(new
                {
                    statusCode = 401,
                    errorMessage = "Invalid credentials"
                });
            }
        }
    }
}
