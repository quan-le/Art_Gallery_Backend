using ArtGallery.Models;
using ArtGallery.Persistence.InterfaceDAO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Persistence
{
    public class TagDAO : ITagDAO

    {
        private readonly GalleryDBContext _context;
        private readonly IMapper _mapper;
        public TagDAO(GalleryDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Tag> GetTags()
        {
            return _context.Tags.AsNoTracking().ToList();
        }
        public Tag GetTagById(Guid id)
        {
            return _context.Tags.AsNoTracking().FirstOrDefault(a => a.tag_id == id);
        }
        // Perform mapping from TagDTO to Tag with its FK
        public Tag MapTagDTOToTag(TagDTO tagDTO)
        {
            var tag = _mapper.Map<Tag>(tagDTO);
            if (tagDTO.artifacts != null)
            {
                foreach (Guid? artifactId in tagDTO.artifacts)
                {
                    var artifact = _context.Artifacts.Find(artifactId);
                    if (artifact != null)
                    {
                        if (tag.artifacts == null)
                        {
                            tag.artifacts = new List<Artifact>();
                        }
                        tag.artifacts.Add(artifact);
                    }
                }
            }
            return tag;
        }
        public Tag AddTag(TagDTO newTagDTO)
        {
            Tag newTag = MapTagDTOToTag(newTagDTO);
            _context.Tags.Add(newTag);
            _context.SaveChanges();
            return newTag;
        }
        public void UpdateTag(Guid id, TagDTO updatedTagDTO)
        {
            Tag updatedTag = MapTagDTOToTag(updatedTagDTO);
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
