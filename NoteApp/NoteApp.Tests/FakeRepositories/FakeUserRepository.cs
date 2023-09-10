using NoteApp.CryptoService;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.Tests.FakeRepositories
{
    public class FakeUserRepository : IUserRepository
    {
        public int userIdTracker = 2;
        private List<User> Users;

        public FakeUserRepository()
        {
            Users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Username = "user1",
                    Age = 18,
                    Password = StringHasher.Hash("user1")
                },
                new User()
                {
                    Id = 2,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Username = "user2",
                    Age = 24,
                    Password = StringHasher.Hash("user2")
                }
            };
        }

        public void Add(User entity)
        {
            entity.Id = ++userIdTracker;
            Users.Add(entity);
        }

        public void Delete(User entity)
        {
            Users.Remove(entity);
        }

        public List<User> GetAll()
        {
            return Users;
        }

        public User GetById(int id)
        {
            return Users
                .SingleOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return Users
                .SingleOrDefault(user => user.Username == username);
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return Users
                .FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == hashedPassword);
        }

        public void SaveChanges() { }

        public void Update(User entity)
        {
            Users[Users.IndexOf(entity)] = entity;
        }
    }
}