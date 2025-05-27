using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class ArtistDAO : IArtistDAO

    {
        private readonly GalleryDBContext _context;
        public ArtistDAO(GalleryDBContext context)
        {
            _context = context;
        }
        public List<Artist> GetArtists()
        {
            return _context.Artists.AsNoTracking().ToList();
        }
        public Artist GetArtistById(Guid id)
        {
            return _context.Artists.AsNoTracking().FirstOrDefault(a => a.artist_id == id);
        }

        public Artist AddArtist(Artist newArtist)
        {
            newArtist.created_date = DateTime.UtcNow;
            newArtist.modified_date = DateTime.UtcNow;
            _context.Artists.Add(newArtist);
            _context.SaveChanges();
            return newArtist;
        }
        public void UpdateArtist(Guid id, Artist updatedArtist)
        {
            var existing = _context.Artists.Find(id);
            if (existing != null)
            {
                //existing.artist_id = updatedArtist.artist_id;
                existing.first_name = updatedArtist.first_name;
                existing.last_name = updatedArtist.last_name;
                existing.gender = updatedArtist.gender;
                existing.birth_date = updatedArtist.birth_date;
                existing.nationality = updatedArtist.nationality;
                existing.modified_date = DateTime.UtcNow;
                existing.biography = updatedArtist.biography;
                _context.SaveChanges();
            }
        }
        public void DeleteArtist(Guid id)
        {
            var existing = _context.Artists.Find(id);
            if (existing != null)
            {
                _context.Artists.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
