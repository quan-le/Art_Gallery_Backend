using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class RoleDAO : IRoleDAO

    {
        private readonly GalleryDBContext _context;
        public RoleDAO(GalleryDBContext context)
        {
            _context = context;
        }
        public List<Role> GetRoles()
        {
            return _context.Roles.AsNoTracking().ToList();
        }
        public Role GetRoleById(Guid id)
        {
            return _context.Roles.AsNoTracking().FirstOrDefault(a => a.role_id == id);
        }

        public Role AddRole(Role newRole)
        {
            _context.Roles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }
        public void UpdateRole(Guid id, Role updatedRole)
        {
            var existing = _context.Roles.Find(id);
            if (existing != null)
            {
                existing.role_id = updatedRole.role_id;
                existing.role_name = updatedRole.role_name;
                existing.description = updatedRole.description;
                existing.UsersRoles = updatedRole.UsersRoles;

                _context.SaveChanges();
            }
        }
        public void DeleteRole(Guid id)
        {
            var existing = _context.Roles.Find(id);
            if (existing != null)
            {
                _context.Roles.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
