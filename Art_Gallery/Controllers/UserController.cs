using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.Persistence.InterfaceDAO;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    [ApiController]
    [Route("api/users")]
    [AllowAnonymous]
    //This is for development purposes only, it is not exposed in the API.
    [ApiExplorerSettings(IgnoreApi = true)]
    [ExcludeFromApiReference]
    public class UserController : ControllerBase
    {
        //Dependency Injection
        private readonly IUserDAO _userDAO;
        public UserController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        // GET All users
        [HttpGet()]
        public IActionResult GetAllUser()
        {
            var users = _userDAO.GetUsers();
            return Ok(users);
        }

        // GET user by ID
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userDAO.GetUserById(id);
            return user != null ? Ok(user) : NotFound();
        }

        // POST add new user
        [HttpPost()]
        public IActionResult AddUser([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("User cannot be null");
            }
            var createduser = _userDAO.AddUser(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = createduser.user_id }, createduser);
        }

        //PUT update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("user cannot be null");
            }
            var existinguser = _userDAO.GetUserById(id);
            if (existinguser == null)
            {
                return NotFound("user with this ID do not exist");
            }
            _userDAO.UpdateUser(id, updatedUser);
            return NoContent();
        }

        //DELETE user 
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var existinguser = _userDAO.GetUserById(id);
            if (existinguser == null)
            {
                return NotFound("user with this ID do not exist");
            }
            _userDAO.DeleteUser(id);
            return NoContent();
        }
    }
}
