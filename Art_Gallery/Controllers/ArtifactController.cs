using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/artifacts")]
    [AllowAnonymous]

    public class ArtifactController : ControllerBase
    {
        //Dependency Injection
        private readonly IArtifactDAO _artifactDAO;
        public ArtifactController(IArtifactDAO artifactDAO)
        {
            _artifactDAO = artifactDAO;
        }

        // GET All Artifacts
        /// <summary>
        /// Retrieve all artifacts in the system.
        /// </summary>
        /// <returns>A list of artifacts</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/artifacts
        /// </remarks>
        /// <response code="200">Returns the list of artifacts</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet()]
        [Authorize(Policy = "User")]
        public IActionResult GetAllArtifact()
        {
            var artifacts = _artifactDAO.GetArtifacts();
            return Ok(artifacts);
        }

        // GET Artifact by ID
        /// <summary>
        /// Retrieve a specific artifact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artifact.</param>
        /// <returns>The artifact details if found</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/artifacts/{id}
        ///     GET /api/artifacts/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="200">Returns the artifact details</response>
        /// <response code="404">If the artifact is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "User")]

        public IActionResult GetArtifact(Guid id)
        {
            var artifact = _artifactDAO.GetArtifactById(id);
            return artifact != null ? Ok(artifact) : NotFound();
        }

        // POST add new Artifact
        /// <summary>
        /// Create a new artifact in the system.
        /// </summary>
        /// <param name="newArtifactDTO">A new Artifact object from the request body.</param>
        /// <returns>The created artifact details</returns>
        /// <response code="201">Returns the newly created artifact</response>
        /// <response code="400">If the provided artifact is null or invalid</response>
        /// <response code="401">If the user is unauthorized to perform this action</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPost()]
        [Authorize(Policy = "Admin")]
        public IActionResult AddArtifact([FromBody] ArtifactDTO newArtifactDTO)
        {
            if (newArtifactDTO == null)
            {
                return BadRequest("Artifact cannot be null");
            }
            var createdArtifact = _artifactDAO.AddArtifact(newArtifactDTO);
            return CreatedAtAction(nameof(GetArtifact), new { id = createdArtifact.artifact_id }, createdArtifact);
        }

        //PUT update Artifact
        /// <summary>
        /// Update an existing artifact by its ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artifact.</param>
        /// <param name="updatedArtifactDTO">The updated Artifact object from the request body.</param>
        /// <returns>No content</returns>
        /// <response code="204">Artifact successfully updated</response>
        /// <response code="400">If the provided artifact is null or invalid</response>
        /// <response code="404">If the artifact with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpdateArtifact(Guid id, [FromBody] ArtifactDTO updatedArtifactDTO)
        {
            if (updatedArtifactDTO == null)
            {
                return BadRequest("Artifact cannot be null");
            }
            var existingArtifact = _artifactDAO.GetArtifactById(id);
            if (existingArtifact == null)
            {
                return NotFound("Artifact with this ID do not exist");
            }
            _artifactDAO.UpdateArtifact(id, updatedArtifactDTO);
            return NoContent();
        }

        //DELETE Artifact 
        /// <summary>
        /// Delete an existing artifact by its ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the artifact.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample Request:
        ///     DELETE /api/artifacts/{id}
        ///     DELETE /api/artifacts/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="204">Artifact successfully deleted</response>
        /// <response code="404">If the artifact with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteArtifact(Guid id)
        {
            var existingArtifact = _artifactDAO.GetArtifactById(id);
            if (existingArtifact == null)
            {
                return NotFound("Artifact with this ID do not exist");
            }
            _artifactDAO.DeleteArtifact(id);
            return NoContent();
        }
    }
}
