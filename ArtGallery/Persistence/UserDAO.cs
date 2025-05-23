using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class UserDAO : IUserDAO

    {
        private readonly GalleryDBContext _context;
        public UserDAO(GalleryDBContext context)
        {
            _context = context;
        }
        public List<User> GetUsers()
        {
            return _context.Users.AsNoTracking().ToList();
        }
        public User GetUserById(Guid id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(a => a.user_id == id);
        }

        public User AddUser(User newUser)
        {
            newUser.created_date = DateTime.UtcNow;
            newUser.modified_date = DateTime.UtcNow;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
        public void UpdateUser(Guid id, User updatedUser)
        {
            var existing = _context.Users.Find(id);
            if (existing != null)
            {
                existing.user_id = updatedUser.user_id;
                existing.email = updatedUser.email;
                existing.password_hash = updatedUser.password_hash;
                existing.first_name = updatedUser.first_name;
                existing.last_name = updatedUser.last_name;
                existing.modified_date = DateTime.UtcNow;
                existing.UsersRoles = updatedUser.UsersRoles;
                _context.SaveChanges();
            }
        }
        public void DeleteUser(Guid id)
        {
            var existing = _context.Users.Find(id);
            if (existing != null)
            {
                _context.Users.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
