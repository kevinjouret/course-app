using CourseAPI.DataContext;
using CourseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CourseDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(CourseDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Permet d'enregistrer un user
        /// Accessible à toute personne même non authentifiée
        /// </summary>
        /// <param name="user">un objet user</param>
        /// <returns>Etat de la création</returns>

        [HttpPost("[Action]")]
        // POST: api/Auth/Register
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var mailCheck = await _context.User.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (mailCheck != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email déjà présent");
            }
            if (user.Address == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Adresse absente");
            }

            var newAddress = new Address()
            {
                Street = user.Address.Street,
                Zipcode = user.Address.Zipcode,
                City = user.Address.City
            };

            var newUser = new User()
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Role = "User",
                Address = newAddress

            };

            try
            {
                _context.User.Add(newUser);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }



        /// <summary>
        /// Permet d'identifier un user
        /// Accessible à toute personne même non authentifiée
        /// </summary>
        /// <param name="user">un objet user</param>
        /// <returns>retourne un token</returns>

        // POST: api/Auth/Login
        [HttpPost("[Action]")]
        public async Task<IActionResult> Login([FromBody] User user)
        {

            bool checkPassword;
            var currentUser = await _context.User.FirstOrDefaultAsync(p => p.Email == user.Email);

            if (currentUser == null)
            {
                return NotFound();
            }
            try
            {
                checkPassword = BCrypt.Net.BCrypt.Verify(user.Password, currentUser.Password);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            if (!checkPassword)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            else
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, currentUser.Role),
                    new Claim("UserId", currentUser.UserId.ToString()),
                    new Claim("Firstname", currentUser.Firstname)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddSeconds(Convert.ToInt32(Convert.ToInt32(_configuration["JWT:TimeBeforeExpirationSeconds"]))),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = currentUser.UserId.ToString(),
                    firstname = currentUser.Firstname.ToString(),
                });
            }
        }
    }

}

