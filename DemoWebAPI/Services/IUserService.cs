using DemoWebAPI.Dtos;
using DemoWebAPI.Models;

namespace DemoWebAPI.Services
{
    public interface IUserService
    {
        public IEnumerable<User> GetUsers();
        public User? GetUserById(int id);
        public User CreateUser(UserDto userDto);
        public void UpdateUser(int id, UserDto userDto);
        public void DeleteUser(int id);
    }
}
