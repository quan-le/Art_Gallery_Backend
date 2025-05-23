using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;

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
        public IActionResult GetAllTag()
        {
            var Tags = _tagDAO.GetTags();
            return Ok(Tags);
        }

        // GET Tag by ID
        [HttpGet("{id}")]
        public IActionResult GetTag(Guid id)
        {
            var Tag = _tagDAO.GetTagById(id);
            return Tag != null ? Ok(Tag) : NotFound();
        }

        // POST add new Tag
        [HttpPost()]
        public IActionResult AddTag([FromBody] Tag newTag)
        {
            if (newTag == null)
            {
                return BadRequest("Tag cannot be null");
            }
            var createdTag = _tagDAO.AddTag(newTag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.tag_id }, createdTag);
        }

        //PUT update Tag
        [HttpPut("{id}")]
        public IActionResult UpdateTag(Guid id, [FromBody] Tag updatedTag)
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
        [HttpDelete("{id}")]
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
