using System;
using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class ArtistDTO
    {
    /// <summary> Constructor for Artist </summary>
    public ArtistDTO() {}
    public ArtistDTO (string? first_name, string? last_name, string? gender, DateOnly? birth_date, string? nationality, string? biography, List<Guid>? artifactsFK )
    {
         this.first_name = first_name;
         this.last_name = last_name;
         this.gender = gender;
         this.birth_date = birth_date;
         this.nationality = nationality;
         this.biography = biography;
         this.artifacts = artifactsFK;
        }

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public string? gender { get; set; }

        public DateOnly? birth_date { get; set; }

        public string? nationality { get; set; }

        public string? biography { get; set; }

        public List<Guid>? artifacts { get; set; }
    }
}
