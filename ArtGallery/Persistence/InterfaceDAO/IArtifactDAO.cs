using ArtGallery.Models;

namespace ArtGallery.Persistence.InterfaceDAO
{
    public interface IArtifactDAO
    {
        Artifact AddArtifact(ArtifactDTO newArtifactDTO);
        void DeleteArtifact(Guid id);
        Artifact GetArtifactById(Guid id);
        //Artifact GetArtifactByTitle(string title);
        List<Artifact> GetArtifacts();
        void UpdateArtifact(Guid id, ArtifactDTO updatedArtifact);
    }
}