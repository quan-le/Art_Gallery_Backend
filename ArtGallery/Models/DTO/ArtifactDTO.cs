using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class ArtifactDTO
    {
    /// <summary> Constructor for Artifact </summary>
        public ArtifactDTO() {}
        public ArtifactDTO(string title, string? description, DateOnly? date_start, DateOnly? date_end, string? date_display, string? material, string? dimension, string? place_of_origin, string? location, double? longitude, double? latitude, string? image_url, List<Guid>? FKArtistObj = null, List<Guid>? FKTagObj = null)
        {
            this.title = title;
            this.description = description;
            this.date_start = date_start;
            this.date_end = date_end;
            this.date_display = date_display;
            this.material = material;
            this.dimension = dimension;
            this.place_of_origin = place_of_origin;
            this.location = location;
            this.longitude = longitude;
            this.latitude = latitude;
            this.image_url = image_url;
            this.artists = FKArtistObj;
            this.tags = FKTagObj;
        }

        public string title { get; set; } = null!;

        public string? description { get; set; }

        public DateOnly? date_start { get; set; }

        public DateOnly? date_end { get; set; }

        public string? date_display { get; set; }

        public string? material { get; set; }

        public string? dimension { get; set; }

        public string? place_of_origin { get; set; }

        public string? location { get; set; }

        public double? longitude { get; set; }

        public double? latitude { get; set; }

        public string? image_url { get; set; }

        //ForeignKey ID
        public  List<Guid>? artists { get; set; }
        public List<Guid>? tags { get; set; }
    }
}
