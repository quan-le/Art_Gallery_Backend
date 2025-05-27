using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class TagDAO : ITagDAO

    {
        private readonly GalleryDBContext _context;
        public TagDAO(GalleryDBContext context)
        {
            _context = context;
        }
        public List<Tag> GetTags()
        {
            return _context.Tags.AsNoTracking().ToList();
        }
        public Tag GetTagById(Guid id)
        {
            return _context.Tags.AsNoTracking().FirstOrDefault(a => a.tag_id == id);
        }

        public Tag AddTag(Tag newTag)
        {
            _context.Tags.Add(newTag);
            _context.SaveChanges();
            return newTag;
        }
        public void UpdateTag(Guid id, Tag updatedTag)
        {
            var existing = _context.Tags.Find(id);
            if (existing != null)
            {
                //existing.tag_id = updatedTag.tag_id;
                existing.tag_name = updatedTag.tag_name;
                existing.tag_description = updatedTag.tag_description;
                existing.artifacts = updatedTag.artifacts;
                _context.SaveChanges();
            }
        }
        public void DeleteTag(Guid id)
        {
            var existing = _context.Tags.Find(id);
            if (existing != null)
            {
                _context.Tags.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
