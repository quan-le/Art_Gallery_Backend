using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class ArtifactDAO<T>
    {
        private readonly GalleryDBContext _context;
        public ArtifactDAO(GalleryDBContext context)
        {
            _context = context;
        }
        public List<Artifact> GetArtifacts()
        {
            return _context.Artifacts.AsNoTracking().ToList();
        }
        public Artifact GetArtifactById(Guid id)
        {
            return _context.Artifacts.AsNoTracking().FirstOrDefault(a => a.artifact_id == id);
        }
        public Artifact GetArtifactByTitle(string title)
        {
            return _context.Artifacts.AsNoTracking().FirstOrDefault(a => a.title == title);
        }
        public Artifact GetArtifactFlexible(string attribute, T value)
        {
            return _context.Artifacts.AsNoTracking().FirstOrDefault(a => EF.Property<T>(a, attribute) == value);
        }
        public void UpdateArtifact(Guid id, Artifact updatedArtifact)
        {
            var existing = _context.Artifacts.Find(id);
            if (existing != null)
            {
                existing.artifact_id = updatedArtifact.artifact_id;
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
