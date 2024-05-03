using BukiApi.Data;
using BukiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BukiApi.Controllers
{

    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext context) 
        { 
            _dbContext = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers() 
        {
            try { 
                var users = await _dbContext.Users.ToListAsync();

                if (users == null) {
                    return NotFound("Nada foi encontrado!");
                }

                return Ok(users);
            } catch (Exception ex) { 
               return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user) 
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user) 
        {
            if (id != user.Id) {
                return BadRequest("O ID enviado não condiz com o id do usuário!");
            }

            _dbContext.Entry(user).State = EntityState.Modified;

            try {
                await _dbContext.SaveChangesAsync();
                return Ok(user);
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            try {
                var user = await _dbContext.Users.FindAsync(id);

                if (user == null) 
                {
                    return NotFound("Usuário não encontrado!");
                }

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool UserExists(int id) {
            return _dbContext.Users.Any(user => user.Id == id);
        }
    }
}
