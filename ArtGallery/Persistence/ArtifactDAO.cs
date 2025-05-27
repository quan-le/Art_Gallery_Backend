using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class ArtifactDAO : IArtifactDAO
    {
        private readonly GalleryDBContext _context;
        private readonly IMapper _mapper;
        public ArtifactDAO(GalleryDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Artifact> GetArtifacts()
        {
            return _context.Artifacts.AsNoTracking().ToList();
        }
        public Artifact GetArtifactById(Guid id)
        {
            return _context.Artifacts.AsNoTracking().FirstOrDefault(a => a.artifact_id == id);
        }
        /*
        public Artifact GetArtifactByTitle(string title)
        {
            return _context.Artifacts.AsNoTracking().FirstOrDefault(a => a.title == title);
        }
        */
        //Perform mapping from ArtifactDTO to Artifact with its FK
        public Artifact MapArtifactDTOToArtifact(ArtifactDTO artifactDTO)
        {
            var artifact = _mapper.Map<Artifact>(artifactDTO);
            if(artifactDTO.artists != null)
            {
                foreach (Guid? artistId in artifactDTO.artists)
                {
                    var artist = _context.Artists.Find(artistId);
                    if (artist != null)
                    {
                        if (artifact.artists == null)
                        {
                            artifact.artists = new List<Artist>();
                        }
                        artifact.artists.Add(artist);
                    }
                }
                
            }
            if (artifactDTO.tags != null)
            {
                foreach(Guid? tagId in artifactDTO.tags)
                {
                    var tag = _context.Tags.Find(tagId);
                    if (tag != null)
                    {
                        if (artifact.tags == null)
                        {
                            artifact.tags = new List<Tag>();
                        }
                        artifact.tags.Add(tag);
                    }
                }
            }
            return artifact;
        }
        public Artifact AddArtifact(ArtifactDTO newArtifactDTO)
        {
            Artifact newArtifact = MapArtifactDTOToArtifact(newArtifactDTO);
            newArtifact.created_date = DateTime.UtcNow;
            newArtifact.modified_date = DateTime.UtcNow;
            _context.Artifacts.Add(newArtifact);
            _context.SaveChanges();
            return newArtifact;
        }
        public void UpdateArtifact(Guid id, ArtifactDTO updatedArtifactDTO)
        {
            Artifact updatedArtifact = MapArtifactDTOToArtifact(updatedArtifactDTO);
            var existing = _context.Artifacts.Find(id);
            if (existing != null)
            {
                
                //existing.artifact_id = updatedArtifact.artifact_id;
                existing.title = updatedArtifact.title;
                existing.description = updatedArtifact.description;
                existing.date_start = updatedArtifact.date_start;
                existing.date_end = updatedArtifact.date_end;
                existing.date_display = updatedArtifact.date_display;
                existing.material = updatedArtifact.material;
                existing.dimension = updatedArtifact.dimension;
                existing.place_of_origin = updatedArtifact.place_of_origin;
                existing.location = updatedArtifact.location;
                existing.longitude = updatedArtifact.longitude;
                existing.latitude = updatedArtifact.latitude;
                existing.image_url = updatedArtifact.image_url;
                existing.modified_date = DateTime.UtcNow;
                existing.artists = updatedArtifact.artists;
                existing.tags = updatedArtifact.tags;
                _context.SaveChanges();
            }
        }
        public void DeleteArtifact(Guid id)
        {
            var existing = _context.Artifacts.Find(id);
            if (existing != null)
            {
                _context.Artifacts.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
