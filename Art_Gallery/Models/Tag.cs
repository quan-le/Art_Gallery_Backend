using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class Tag
    {
    /// <summary> Constructor for Tag </summary>
    public Tag() {}
    public Tag (Guid tag_id, string tag_name, string? tag_description, ICollection<Artifact> artifacts = null )
    {
        this.tag_id = tag_id;
        this.tag_name = tag_name;
        this.tag_description = tag_description;
        this.artifacts = artifacts;
    }
        public Guid? tag_id { get; set; }

        public string tag_name { get; set; } = null!;

        public string? tag_description { get; set; }

        public virtual ICollection<Artifact>? artifacts { get; set; }
    }
}
