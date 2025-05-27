using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class Artist
    {
    /// <summary> Constructor for Artist </summary>
    public Artist() {}
    public Artist (Guid artist_id, string? first_name, string? last_name, string? gender, DateOnly? birth_date, string? nationality, DateTime? created_date, DateTime? modified_date, string? biography, ICollection<Artifact> artifacts = null)
    {
         this.artist_id = artist_id;
         this.first_name = first_name;
         this.last_name = last_name;
         this.gender = gender;
         this.birth_date = birth_date;
         this.nationality = nationality;
         this.created_date = created_date;
         this.modified_date = modified_date;
         this.biography = biography;
         this.artifacts = artifacts;
        }
        public Guid? artist_id { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public string? gender { get; set; }

        public DateOnly? birth_date { get; set; }

        public string? nationality { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public string? biography { get; set; }

        public virtual ICollection<Artifact>? artifacts { get; set; }
    }
}
