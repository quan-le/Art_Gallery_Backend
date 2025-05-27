using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    //This is for development purposes only, it is not exposed in the API.
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsersRole
    {
    /// <summary> Constructor for UsersRole </summary>
    public UsersRole() {}
    public UsersRole (Guid user_id, Guid role_id, DateTime? assigned_date )
    {
         this.user_id = user_id;
         this.role_id = role_id;
         this.assigned_date = assigned_date;
    }
        public Guid user_id { get; set; }

        public Guid role_id { get; set; }

        public DateTime? assigned_date { get; set; }

    public virtual Role role { get; set; } = null!;
    public virtual User user { get; set; } = null!;
    }
}
