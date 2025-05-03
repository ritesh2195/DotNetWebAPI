using DemoWebAPI.Models;

namespace DemoWebAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User? GetUserById(int id);
    }
}
