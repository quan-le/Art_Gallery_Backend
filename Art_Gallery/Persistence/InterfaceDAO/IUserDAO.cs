using ArtGallery.Models;

namespace ArtGallery.Persistence.InterfaceDAO
{
    public interface IUserDAO
    {
        User AddUser(User newUser);
        void DeleteUser(Guid id);
        User GetUserById(Guid id);
        List<User> GetUsers();
        void UpdateUser(Guid id, User updatedUser);
    }
}