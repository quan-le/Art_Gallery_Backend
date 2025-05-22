using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class Role
    {
    /// <summary> Constructor for Role </summary>
    public Role() {}
    public Role (Guid role_id, string role_name, string? description )
    {
         this.role_id = role_id;
         this.role_name = role_name;
         this.description = description;
    }
        public Guid role_id { get; set; }

        public string role_name { get; set; } = null!;

        public string? description { get; set; }

        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}
