using ArtGallery.Persistence;
using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/artifacts")]
    public class ArtifactController : ControllerBase
    {
        //Dependency Injection
        private readonly IArtifactDAO _artifactDAO;
        public ArtifactController(IArtifactDAO artifactDAO)
        {
            _artifactDAO = artifactDAO;
        }

        // GET All Artifacts
        [HttpGet()]
        public IActionResult GetAllArtist()
        {
            var artifacts = _artifactDAO.GetArtifacts();
            return Ok(artifacts);
        }

        // GET Artifact by ID
        [HttpGet("{id}")]
        public IActionResult GetArtifact(Guid id)
        {
            var artifact = _artifactDAO.GetArtifactById(id);
            return artifact != null ? Ok(artifact) : NotFound();
        }

        // POST add new Artifact
        [HttpPost()]
        public IActionResult AddArtifact([FromBody] Artifact newArtifact)
        {
            if (newArtifact == null)
            {
                return BadRequest("Artifact cannot be null");
            }
            var createdArtifact = _artifactDAO.AddArtifact(newArtifact);
            return CreatedAtAction(nameof(GetArtifact), new { id = createdArtifact.artifact_id }, createdArtifact);
        }

        //PUT update Artifact
        [HttpPut("{id}")]
        public IActionResult UpdatArtifact(Guid id, [FromBody] Artifact updatedArtifact)
        {
            if (updatedArtifact == null)
            {
                return BadRequest("Artifact cannot be null");
            }
            var existingArtifact = _artifactDAO.GetArtifactById(id);
            if (existingArtifact == null)
            {
                return NotFound("Artifact with this ID do not exist");
            }
            _artifactDAO.UpdateArtifact(id, updatedArtifact);
            return NoContent();
        }

        //DELETE Artifact 
        [HttpDelete("{id}")]
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
