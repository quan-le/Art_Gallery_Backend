using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
namespace ArtGallery.Persistence
{
    public class ArtistDAO : IArtistDAO

    {
        private readonly GalleryDBContext _context;
        private readonly IMapper _mapper;
        public ArtistDAO(GalleryDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Artist> GetArtists()
        {
            return _context.Artists.AsNoTracking().ToList();
        }
        public Artist GetArtistById(Guid id)
        {
            return _context.Artists.AsNoTracking().FirstOrDefault(a => a.artist_id == id);
        }
        //Mapping ArtistDTO to Artist with its FK
        public Artist MapArtistDTOToArtist(ArtistDTO artistDTO)
        {
            var artist = _mapper.Map<Artist>(artistDTO);
            if (artistDTO.artifacts != null)
            {
                foreach (Guid? artifactId in artistDTO.artifacts)
                {
                    var artifact = _context.Artifacts.Find(artifactId);
                    if (artifact != null)
                    {
                        if (artist.artifacts == null)
                        {
                            artist.artifacts = new List<Artifact>();
                        }
                        artist.artifacts.Add(artifact);
                    }
                }
            }
            return artist;
        }
        public Artist AddArtist(ArtistDTO newArtist)
        {
            Artist artist = MapArtistDTOToArtist(newArtist);
            artist.created_date = DateTime.UtcNow;
            artist.modified_date = DateTime.UtcNow;
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return artist;
        }
        public void UpdateArtist(Guid id, ArtistDTO updatedArtistDTO)
        {
            Artist updatedArtist = MapArtistDTOToArtist(updatedArtistDTO);
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
                existing.artifacts = updatedArtist.artifacts;
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
