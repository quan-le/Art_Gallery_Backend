using ArtGallery.Models;

namespace ArtGallery.Persistence.InterfaceDAO
{
    public interface IArtistDAO
    {
        Artist AddArtist(ArtistDTO newArtistDTO);
        void DeleteArtist(Guid id);
        Artist GetArtistById(Guid id);
        List<Artist> GetArtists();
        void UpdateArtist(Guid id, ArtistDTO updatedArtistDTO);
    }
}