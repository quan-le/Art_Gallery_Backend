using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;
using Scalar.AspNetCore;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/roles")]
    //This is for development purposes only, it is not exposed in the API.
    [ApiExplorerSettings(IgnoreApi = true)]
    [ExcludeFromApiReference]
    public class RoleController : ControllerBase
    {
        //Dependency Injection
        private readonly IRoleDAO _roleDAO;
        public RoleController(IRoleDAO RoleDAO)
        {
            _roleDAO = RoleDAO;
        }

        // GET All Roles
        [HttpGet()]
        public IActionResult GetAllRole()
        {
            var Roles = _roleDAO.GetRoles();
            return Ok(Roles);
        }

        // GET Role by ID
        [HttpGet("{id}")]
        public IActionResult GetRole(Guid id)
        {
            var Role = _roleDAO.GetRoleById(id);
            return Role != null ? Ok(Role) : NotFound();
        }

        // POST add new Role
        [HttpPost()]
        public IActionResult AddRole([FromBody] Role newRole)
        {
            if (newRole == null)
            {
                return BadRequest("Role cannot be null");
            }
            var createdRole = _roleDAO.AddRole(newRole);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.role_id }, createdRole);
        }

        //PUT update Role
        [HttpPut("{id}")]
        public IActionResult UpdateRole(Guid id, [FromBody] Role updatedRole)
        {
            if (updatedRole == null)
            {
                return BadRequest("Role cannot be null");
            }
            var existingRole = _roleDAO.GetRoleById(id);
            if (existingRole == null)
            {
                return NotFound("Role with this ID do not exist");
            }
            _roleDAO.UpdateRole(id, updatedRole);
            return NoContent();
        }

        //DELETE Role 
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(Guid id)
        {
            var existingRole = _roleDAO.GetRoleById(id);
            if (existingRole == null)
            {
                return NotFound("Role with this ID do not exist");
            }
            _roleDAO.DeleteRole(id);
            return NoContent();
        }
    }
}
