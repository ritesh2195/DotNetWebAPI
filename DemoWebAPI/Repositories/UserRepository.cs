using DemoWebAPI.Data;
using DemoWebAPI.Models;

namespace DemoWebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(IConfiguration config)
        {
            _dbContext = new AppDbContext(config);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _dbContext.Add(entityToAdd);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            List<User> users = _dbContext
                .Users
                .Where(x => x.IsDeleted == false)
                .ToList();

            return users;
        }

        public User? GetUserById(int id)
        {

            return _dbContext.Users
                .Where(x => x.UserMasterId == id && x.IsDeleted == false)
                .FirstOrDefault();
        }
    }
}
