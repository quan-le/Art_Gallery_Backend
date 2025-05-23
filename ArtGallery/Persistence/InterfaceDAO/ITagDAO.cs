using ArtGallery.Models;

namespace ArtGallery.Persistence.InterfaceDAO
{
    public interface ITagDAO
    {
        Tag AddTag(Tag newTag);
        void DeleteTag(Guid id);
        Tag GetTagById(Guid id);
        List<Tag> GetTags();
        void UpdateTag(Guid id, Tag updatedTag);
    }
}