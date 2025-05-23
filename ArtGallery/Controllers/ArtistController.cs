using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/artists")]
    public class ArtistController : ControllerBase
    {
        //Dependency Injection
        private readonly IArtistDAO _artistDAO;
        public ArtistController(IArtistDAO artistDAO)
        {
            _artistDAO = artistDAO;
        }

        // GET All artists
        [HttpGet()]
        public IActionResult GetAllArtist()
        {
            var artists = _artistDAO.GetArtists();
            return Ok(artists);
        }

        // GET Artist by ID
        [HttpGet("{id}")]
        public IActionResult GetArtist(Guid id)
        {
            var Artist = _artistDAO.GetArtistById(id);
            return Artist != null ? Ok(Artist) : NotFound();
        }

        // POST add new Artist
        [HttpPost()]
        public IActionResult AddArtist([FromBody] Artist newArtist)
        {
            if (newArtist == null)
            {
                return BadRequest("Artist cannot be null");
            }
            var createdArtist = _artistDAO.AddArtist(newArtist);
            return CreatedAtAction(nameof(GetArtist), new { id = createdArtist.artist_id }, createdArtist);
        }

        //PUT update Artist
        [HttpPut("{id}")]
        public IActionResult UpdatArtist(Guid id, [FromBody] Artist updatedArtist)
        {
            if (updatedArtist == null)
            {
                return BadRequest("Artist cannot be null");
            }
            var existingArtist = _artistDAO.GetArtistById(id);
            if (existingArtist == null)
            {
                return NotFound("Artist with this ID do not exist");
            }
            _artistDAO.UpdateArtist(id, updatedArtist);
            return NoContent();
        }

        //DELETE Artist 
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(Guid id)
        {
            var existingArtist = _artistDAO.GetArtistById(id);
            if (existingArtist == null)
            {
                return NotFound("Artist with this ID do not exist");
            }
            _artistDAO.DeleteArtist(id);
            return NoContent();
        }
    }
}
