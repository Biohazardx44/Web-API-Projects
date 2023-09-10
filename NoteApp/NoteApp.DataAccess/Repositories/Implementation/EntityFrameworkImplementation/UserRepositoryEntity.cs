using NoteApp.DataAccess.Data;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation
{
    public class UserRepositoryEntity : IUserRepository
    {
        private readonly NoteAppDbContext _noteAppDbContext;

        public UserRepositoryEntity(NoteAppDbContext noteAppDbContext)
        {
            _noteAppDbContext = noteAppDbContext;
        }

        public void Add(User entity)
        {
            _noteAppDbContext.Users.Add(entity);
            _noteAppDbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _noteAppDbContext.Users.Remove(entity);
            _noteAppDbContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _noteAppDbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _noteAppDbContext.Users
                    .SingleOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _noteAppDbContext.Users
                    .SingleOrDefault(user => user.Username == username);
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _noteAppDbContext.Users
                    .FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == hashedPassword);
        }

        public void SaveChanges()
        {
            _noteAppDbContext.SaveChanges();
        }

        public void Update(User entity)
        {
            _noteAppDbContext.Users.Update(entity);
            _noteAppDbContext.SaveChanges();
        }
    }
}