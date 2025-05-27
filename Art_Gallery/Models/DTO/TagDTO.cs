using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class TagDTO
    {
    /// <summary> Constructor for Tag </summary>
    public TagDTO() {}
    public TagDTO ( string tag_name, string? tag_description, List<Guid>? artifactFK )
    {
         this.tag_name = tag_name;
         this.tag_description = tag_description;
         this.artifacts = artifactFK;
        }

        public string tag_name { get; set; } = null!;

        public string? tag_description { get; set; }

        public List<Guid>? artifacts { get; set; }
    }
}
