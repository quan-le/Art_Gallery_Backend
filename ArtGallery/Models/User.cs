using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class User
    {
    /// <summary> Constructor for User </summary>
    public User() {}
    public User (Guid user_id, string email, string password_hash, string? first_name, string? last_name, DateTime created_date, DateTime modified_date )
    {
         this.user_id = user_id;
         this.email = email;
         this.password_hash = password_hash;
         this.first_name = first_name;
         this.last_name = last_name;
         this.created_date = created_date;
         this.modified_date = modified_date;
    }
        public Guid user_id { get; set; }

        public string email { get; set; } = null!;

        public string password_hash { get; set; } = null!;

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public DateTime created_date { get; set; }

        public DateTime modified_date { get; set; }

        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}
