using DemoWebAPI.Dtos;
using DemoWebAPI.Models;
using DemoWebAPI.Repositories;
using System.Text.RegularExpressions;

namespace DemoWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }
        public User? GetUserById(int id)
        {
            User? user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User ID is not valid or deleted");
            }

            return user;
        }
        public User CreateUser(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User DTO cannot be null.");
            }

            var users = _userRepository.GetUsers();

            foreach (User user in users)
            {
                if (user.Email == userDto.Email)
                {
                    throw new ArgumentException("Email already exists.");
                }
            }

            if (string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                throw new ArgumentException("First Name is required.");
            }

            if (userDto.FirstName.Length > 50)
            {
                throw new ArgumentException("First Name should not exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(userDto.LastName))
            {
                throw new ArgumentException("Last Name is required.");
            }

            if (userDto.LastName.Length > 50)
            {
                throw new ArgumentException("Last Name should not exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (userDto.Email.Length > 100)
            {
                throw new ArgumentException("Email should not exceed 100 characters.");
            }

            if (!Regex.IsMatch(userDto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                throw new ArgumentException("Email is not in a valid format.");
            }

            if (string.IsNullOrEmpty(userDto.Gender))
            {
                throw new ArgumentException("Gender is required.");
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
                return userDb;
            }

            throw new Exception("Failed to create the user");

        }

        public void UpdateUser(int id, UserDto userDto)
        {
            User? user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException("User ID is not valid or deleted");
            }

            if (string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                throw new ArgumentException("First Name is required.");
            }

            if (userDto.FirstName.Length > 50)
            {
                throw new ArgumentException("First Name should not exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(userDto.LastName))
            {
                throw new ArgumentException("Last Name is required.");
            }

            if (userDto.LastName.Length > 50)
            {
                throw new ArgumentException("Last Name should not exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (userDto.Email.Length > 100)
            {
                throw new ArgumentException("Email should not exceed 100 characters.");
            }

            if (!Regex.IsMatch(userDto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                throw new ArgumentException("Email is not in a valid format.");
            }

            if (string.IsNullOrEmpty(userDto.Gender))
            {
                throw new ArgumentException("Gender is required.");
            }

            user.FirstName = userDto.FirstName;

            user.LastName = userDto.LastName;

            user.Email = userDto.Email;

            user.Gender = userDto.Gender;

            user.IsActive = userDto.IsActive;

            user.IsDeleted = false;

            if (_userRepository.SaveChanges())
            {
                return;
            }

            throw new Exception("Failed to update the user");
        }

        public void DeleteUser(int id)
        {
            User? user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException("User ID is not valid or deleted");
            }

            user.IsDeleted = true;

            if (_userRepository.SaveChanges())
            {
                return;
            }

            throw new Exception("Failed to delete the user");
        }
    }
}
