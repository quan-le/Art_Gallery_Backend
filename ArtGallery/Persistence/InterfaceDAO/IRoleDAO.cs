using ArtGallery.Models;

namespace ArtGallery.Persistence.InterfaceDAO
{
    public interface IRoleDAO
    {
        Role AddRole(Role newRole);
        void DeleteRole(Guid id);
        Role GetRoleById(Guid id);
        List<Role> GetRoles();
        void UpdateRole(Guid id, Role updatedRole);
    }
}