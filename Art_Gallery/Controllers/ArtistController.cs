using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    [ApiController]
    [AllowAnonymous]

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
        /// <summary>
        /// Retrieve all artists in the system.
        /// </summary>
        /// <returns>A list of artists</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/artists
        /// </remarks>
        /// <response code="200">Returns the list of artists</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet()]
        [Authorize(Policy = "User")]

        public IActionResult GetAllArtist()
        {
            var artists = _artistDAO.GetArtists();
            return Ok(artists);
        }

        // GET Artist by ID
        /// <summary>
        /// Retrieve a specific artist by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artist.</param>
        /// <returns>The artist details if found</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/artists/{id}
        ///     GET /api/artists/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="200">Returns the artist details</response>
        /// <response code="404">If the artist is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "User")]

        public IActionResult GetArtist(Guid id)
        {
            var Artist = _artistDAO.GetArtistById(id);
            return Artist != null ? Ok(Artist) : NotFound();
        }

        // POST add new Artist
        /// <summary>
        /// Create a new artist in the system.
        /// </summary>
        /// <param name="newArtistDTO">A new Artist object from the request body.</param>
        /// <returns>The created artist details</returns>
        /// <remarks>
        /// Sample Request:
        ///     POST /api/artists/{id}
        ///     {
        ///           "first_name": "Leonardo",
        ///           "last_name": "da Vinci",
        ///           "gender": "Male",
        ///           "birth_date": "1452-04-15",
        ///           "nationality": "Italian",
        ///           "biography": "Polymath of the High Renaissance—painter, engineer, and scientist.",
        ///           "artifacts": ["f6809a24-84a3-42cb-b6ac-2342f574918f"]"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created artist</response>
        /// <response code="400">If the provided artist is null or invalid</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPost()]
        [Authorize(Policy = "Admin")]

        public IActionResult AddArtist([FromBody] ArtistDTO newArtistDTO)
        {
            if (newArtistDTO == null)
            {
                return BadRequest("Artist cannot be null");
            }
            var createdArtist = _artistDAO.AddArtist(newArtistDTO);
            return CreatedAtAction(nameof(GetArtist), new { id = createdArtist.artist_id }, createdArtist);
        }

        //PUT update Artist
        /// <summary>
        /// Update an existing artist by their ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artist.</param>
        /// <param name="updatedArtistDTO">The updated Artist object from the request body.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample Request:
        ///     PUT /api/artists/{id}
        ///     {
        ///           "first_name": "Leonardo",
        ///           "last_name": "da Vinci",
        ///           "gender": "Male",
        ///           "birth_date": "1452-04-15",
        ///           "nationality": "Italian",
        ///           "biography": "Polymath of the High Renaissance—painter, engineer, and scientist.",
        ///           "artifacts": ["f6809a24-84a3-42cb-b6ac-2342f574918f"]"
        ///     }
        /// </remarks>
        /// <response code="204">Artist successfully updated</response>
        /// <response code="400">If the provided artist is null or invalid</response>
        /// <response code="404">If the artist with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]

        public IActionResult UpdateArtist(Guid id, [FromBody] ArtistDTO updatedArtistDTO)
        {
            if (updatedArtistDTO == null)
            {
                return BadRequest("Artist cannot be null");
            }
            var existingArtist = _artistDAO.GetArtistById(id);
            if (existingArtist == null)
            {
                return NotFound("Artist with this ID do not exist");
            }
            _artistDAO.UpdateArtist(id, updatedArtistDTO);
            return NoContent();
        }

        //DELETE Artist 
        /// <summary>
        /// Delete an existing artist by their ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artist.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample Request:
        ///     DELETE /api/artists/{id}
        ///     DELETE /api/artists/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="204">Artist successfully deleted</response>
        /// <response code="404">If the artist with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
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
