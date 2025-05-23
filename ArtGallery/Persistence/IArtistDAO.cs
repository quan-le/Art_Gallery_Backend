using ArtGallery.Models;

namespace ArtGallery.Persistence
{
    public interface IArtistDAO
    {
        Artist AddArtist(Artist newArtist);
        void DeleteArtist(Guid id);
        Artist GetArtistById(Guid id);
        List<Artist> GetArtists();
        void UpdateArtist(Guid id, Artist updatedArtist);
    }
}