using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController : ControllerBase
    {
        //Dependency Injection
        private readonly ITagDAO _tagDAO;
        public TagController(ITagDAO TagDAO)
        {
            _tagDAO = TagDAO;
        }

        // GET All Tags
        [HttpGet()]
        [Authorize(Policy = "User")]

        /// <summary>
        /// GET all tags in the database.
        /// </summary>
        /// <returns>A list of tags</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/tags
        /// </remarks>
        /// <response code="200">Returns the list of tags</response>
        /// <response code="500">If an internal server error occurs</response>
        public IActionResult GetAllTag()
        {
            var Tags = _tagDAO.GetTags();
            return Ok(Tags);
        }

        // GET Tag by ID
        /// <summary>
        /// GET a tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the tag.</param>
        /// <returns>The tag details if found</returns>
        /// <remarks>
        /// Sample Request:
        ///     GET /api/tags/{id}
        ///     GET /api/tags/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="200">Returns the tag details</response>
        /// <response code="404">If the tag is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "User")]

        public IActionResult GetTag(Guid id)
        {
            var Tag = _tagDAO.GetTagById(id);
            return Tag != null ? Ok(Tag) : NotFound();
        }

        // POST add new Tag
        /// <summary>
        /// Add a new tag to the system.
        /// </summary>
        /// <param name="newTag">A new Tag object from the request body.</param>
        /// <returns>The created tag details</returns>
        /// <remarks>
        /// Sample Request:
        ///     POST /api/tags
        ///     {
        ///         "tag_name": "Impressionism"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created tag</response>
        /// <response code="400">If the provided tag is null or invalid</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPost()]
        [Authorize (Policy = "Admin")]
        public IActionResult AddTag([FromBody] TagDTO newTag)
        {
            if (newTag == null)
            {
                return BadRequest("Tag cannot be null");
            }
            var createdTag = _tagDAO.AddTag(newTag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.tag_id }, createdTag);
        }

        //PUT update Tag
        /// <summary>
        /// Update an existing tag by its ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the tag.</param>
        /// <param name="updatedTag">The updated Tag object from the request body.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample Request:
        ///     PUT /api/tags/{id}
        ///     {
        ///         "tag_name": "Modern Art"
        ///     }
        /// </remarks>
        /// <response code="204">Tag successfully updated</response>
        /// <response code="400">If the provided tag is null or invalid</response>
        /// <response code="404">If the tag with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]

        public IActionResult UpdateTag(Guid id, [FromBody] TagDTO updatedTag)
        {
            if (updatedTag == null)
            {
                return BadRequest("Tag cannot be null");
            }
            var existingTag = _tagDAO.GetTagById(id);
            if (existingTag == null)
            {
                return NotFound("Tag with this ID do not exist");
            }
            _tagDAO.UpdateTag(id, updatedTag);
            return NoContent();
        }

        //DELETE Tag 
        /// <summary>
        /// Delete an existing tag by its ID.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the tag.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample Request:
        ///     DELETE /api/tags/{id}
        ///     DELETE /api/tags/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <response code="204">Tag successfully deleted</response>
        /// <response code="404">If the tag with the given ID does not exist</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]

        public IActionResult DeleteTag(Guid id)
        {
            var existingTag = _tagDAO.GetTagById(id);
            if (existingTag == null)
            {
                return NotFound("Tag with this ID do not exist");
            }
            _tagDAO.DeleteTag(id);
            return NoContent();
        }
    }
}
