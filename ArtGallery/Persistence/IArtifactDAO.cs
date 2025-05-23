using ArtGallery.Models;

namespace ArtGallery.Persistence
{
    public interface IArtifactDAO
    {
        Artifact AddArtifact(Artifact newArtifact);
        void DeleteArtifact(Guid id);
        Artifact GetArtifactById(Guid id);
        //Artifact GetArtifactByTitle(string title);
        List<Artifact> GetArtifacts();
        void UpdateArtifact(Guid id, Artifact updatedArtifact);
    }
}