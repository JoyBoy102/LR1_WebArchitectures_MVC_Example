using LR1.Interfaces;
using LR1.Services;
using Microsoft.AspNetCore.Mvc;
namespace LR1.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service) => _service = service;

        // GET /api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_service.GetAll());
        }

        // GET /api/users/1
        [HttpGet("{id:int}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _service.GetById(id);
            if (user is null)
                return NotFound(new { error = "User not found" });

            return Ok(user);
        }

        // POST /api/users
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { error = "Name and email are required" });
            }

            var created = _service.Create(request.Name, request.Email);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }

        // PUT /api/users/1
        [HttpPut("{id:int}")]
        public ActionResult<User> UpdateUser(int id, [FromBody] CreateUserRequest request)
        {
            var updated = _service.Update(id, request.Name, request.Email);
            if (updated is null)
                return NotFound(new { error = "User not found" });

            return Ok(updated);
        }

        // DELETE /api/users/1
        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var ok = _service.Delete(id);
            if (!ok)
                return NotFound(new { error = "User not found" });

            return NoContent();
        }

        public record CreateUserRequest(string Name, string Email);
    }
}
