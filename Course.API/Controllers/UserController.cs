using CourseAPI.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CourseDbContext _context;

        public UserController(CourseDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Modifie un user
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <param name="userId">identifiant d'un user</param>
        /// <param name="user">Objet user</param>
        /// <returns>un no content</returns>

        [Authorize(Roles = "Admin")]
        // PUT: api/User/5
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(int userId, User user)
        {
            if (userId != user.UserId)
            {
                return BadRequest();
            }

            var currentUser = await _context.User.Include(a => a.Address).FirstOrDefaultAsync(u => u.UserId == userId);

            currentUser.Lastname = user.Lastname;
            currentUser.Firstname = user.Firstname;
            currentUser.Email = user.Email;
            currentUser.Role = user.Role;

            try
            {
                if (BCrypt.Net.BCrypt.Verify(user.Password, currentUser.Password))
                {
                    currentUser.Password = user.Password;
                }
                else
                    currentUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            if (user.Address != null)
            {
                //eviter les erreurs de clef externe
                currentUser.Address.AddressId = userId;
                currentUser.Address.City = user.Address.City;
                currentUser.Address.Zipcode = user.Address.Zipcode;
                currentUser.Address.City = user.Address.City;
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                if (!UserExists(userId))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }



        /// <summary>
        /// Modifie les informations de l'user authentifié
        /// Accessible seulement aux utilisateurs authentifiés
        /// </summary>
        /// <param name="user">Objet user</param>
        /// <returns>No content (204)</returns>

        [Authorize]
        // PUT: api/User/PutUserAuth
        [HttpPut("[Action]")]
        public async Task<IActionResult> PutUserAuth(User user)
        {
            string currentId = "0";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (Convert.ToInt32(currentId) != user.UserId)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var currentUser = await _context.User.Include(a => a.Address).FirstOrDefaultAsync(u => u.UserId == user.UserId);

            //sécurise le fait que l'user ne changera pas son rôle en Admin
            currentUser.Role = "User";

            try
            {
                if (BCrypt.Net.BCrypt.Verify(user.Password, currentUser.Password))
                {
                    currentUser.Password = user.Password;
                }
                else
                    currentUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            currentUser.Lastname = user.Lastname;
            currentUser.Firstname = user.Firstname;
            currentUser.Email = user.Email;

            if (user.Address != null)
            {
                //eviter les erreurs de clef externe
                currentUser.Address.AddressId = Convert.ToInt32(currentId);
                currentUser.Address.Street = user.Address.Street;
                currentUser.Address.Zipcode = user.Address.Zipcode;
                currentUser.Address.City = user.Address.City;
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                if (!UserExists(Convert.ToInt32(currentId)))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }



        /// <summary>
        /// Récupère la liste des utilisateurs ordonnées par nom et prénom
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <returns>liste d'utilisateurs</returns>

        [Authorize(Roles = "Admin")]
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.OrderBy(n => n.Lastname).ThenBy(p => p.Firstname).ToListAsync();
        }


        /// <summary>
        /// Récupère les informations sur un user
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <param name="id">identifiant d'un user</param>
        /// <returns>un user</returns>

        [Authorize(Roles = "Admin")]
        // GET: api/User/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _context.User.Include(a => a.Address).FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        /// <summary>
        /// Récupère les informations sur un user authentifié
        /// Accessible seulement aux personnes authentifiées
        /// </summary>
        /// <returns>un user</returns>

        [Authorize]
        // GET: api/User/GetUserAuth
        [HttpGet("[Action]")]
        public async Task<ActionResult<User>> GetUserAuth()
        {
            string currentId = "0";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (Convert.ToInt32(currentId) == 0)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var user = await _context.User.Include(a => a.Address).FirstOrDefaultAsync(u => u.UserId == Convert.ToInt32(currentId));

            if (user == null)
            {
                return NotFound();
            }
            else
                return user;
        }



        /// <summary>
        /// crée un user
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <param name="user">objet user</param>
        /// <returns>un user</returns>

        [Authorize(Roles = "Admin")]
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        /// <summary>
        /// supprime un user
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <param name="id">identifiant d'user</param>
        /// <returns>no content</returns>

        [Authorize(Roles = "Admin")]
        // DELETE: api/User/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        private bool UserExists(int userId)
        {
            return _context.User.Any(e => e.UserId == userId);
        }
    }
}

