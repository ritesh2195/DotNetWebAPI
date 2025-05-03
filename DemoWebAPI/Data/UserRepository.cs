using DemoWebAPI.Models;

namespace DemoWebAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dataContextEF;
        public UserRepository(IConfiguration config)
        {
            _dataContextEF = new DbContext(config);
        }

        public bool SaveChanges()
        {
            return _dataContextEF.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _dataContextEF.Add(entityToAdd);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            List<User> users = _dataContextEF
                .Users
                .Where(x => x.IsDeleted == false)
                .ToList();

            return users;
        }

        public User? GetUserById(int id)
        {

            return _dataContextEF.Users
                .Where(x => x.UserMasterId == id && x.IsDeleted == false)
                .FirstOrDefault();
        }
    }
}
