using CourseAPI.DataContext;
using CourseAPI.Models;
using CourseAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly CourseDbContext _context;

        public CourseController(CourseDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retourne les Course basés sur de la pagination et le triés.
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <param name="pageNum">Index de page</param>
        /// <param name="pageSize">Taille de la Page</param>
        /// <param name="sort">Colonne de sort : name, price, rate</param>
        /// <param name="order">True pour trier par order croissant</param>
        /// <param name="keyword">Chaine recherchée</param>
        /// <returns>Liste de Course</returns>

        // GET: api/Course
        [Authorize]
        [HttpGet("[Action]")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(string sort, bool order, string keyword, int pageNum, int pageSize)
        {
            IQueryable<Course> res;

            switch (sort)
            {
                case "name":
                    if (order)
                        res = (IQueryable<Course>)_context.Course.OrderBy(t => t.Name).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = (IQueryable<Course>)_context.Course.OrderByDescending(t => t.Name).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                case "price":
                    if (order)
                        res = (IQueryable<Course>)_context.Course.OrderBy(t => t.Price).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = (IQueryable<Course>)_context.Course.OrderByDescending(t => t.Price).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                case "rate":
                    if (order)
                        res = (IQueryable<Course>)_context.Course.OrderBy(t => t.Rate).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = (IQueryable<Course>)_context.Course.OrderByDescending(t => t.Rate).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    res = (IQueryable<Course>)_context.Course.Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
            }

            res = res.Include(p => p.Languages.OrderBy(l => l.Name));

            if (!String.IsNullOrEmpty(keyword))
                res = res.Where(p => p.Name.Contains(keyword));

            if (!res.Any())
            {
                return NotFound();
            }
            else
                return Ok(await res.ToListAsync());

        }



        /// <summary>
        /// Retourne un Course basés sur son identifiant
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <param name="courseId">identifiant de course</param>
        /// <returns>Retourne un Course</returns>

        [Authorize]
        // GET: api/Course/5
        [HttpGet("[Action]/{courseId}")]
        public async Task<ActionResult<Course>> GetCours(int courseId)
        {
            var course = await _context.Course.Include(p => p.Languages).FirstOrDefaultAsync(p => p.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }



        /// <summary>
        /// Retourne pour un utilisateur authentifié ses course achetés.
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <param name="pageNum">Index de page</param>
        /// <param name="pageSize">Taille de la Page</param>
        /// <param name="sort">Colonne de sort : name, price, rate</param>
        /// <param name="order">True pour trier par order croissant</param>
        /// <param name="keyword">Chaine recherchée</param>
        /// <returns>Liste de course</returns>

        [Authorize]
        // GET: api/Course/GetCoursesAuth
        [HttpGet("[Action]")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesAuth(string sort, bool order, string keyword, int pageNum, int pageSize)
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


            var res = _context.Course.Join(_context.Purchase.Where(p => p.UserId == Convert.ToInt32(currentId)),
                                                    c => c.CourseId,
                                                    a => a.CourseId,
                                                    (a, c) =>
                                                        new { a.CourseId, a.Name, a.Description, a.Rate, a.ImageUrl, a.VideoUrl, a.Price, a.Languages });

            switch (sort)
            {
                case "name":
                    if (order)
                        res = res.OrderBy(t => t.Name).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = res.OrderByDescending(t => t.Name).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                case "price":
                    if (order)
                        res = res.OrderBy(t => t.Price).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = res.OrderByDescending(t => t.Price).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                case "rate":
                    if (order)
                        res = res.OrderBy(t => t.Rate).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    else
                        res = res.OrderByDescending(t => t.Rate).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    res = res.Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
            }

            if (!String.IsNullOrEmpty(keyword))
                res = res.Where(p => p.Name.Contains(keyword));



            if (res.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(await res.ToListAsync());
        }



        /// <summary>
        /// Mise à jour d'un course
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="course">objet course</param>
        /// /// <param name="courseId">Id du course</param>
        /// <returns>Retourne un Course</returns>

        // PUT: api/Course/5
        [Authorize(Roles = "Admin")]
        [HttpPut("[Action]/{courseId}")]
        public async Task<IActionResult> PutCourse(int courseId, [FromForm] Course course)
        {
            List<Language> languages = (List<Language>)course.Languages;

            string filePath = string.Empty;

            if (courseId != course.CourseId)
            {
                return BadRequest();
            }

            var currentCourse = await _context.Course.FindAsync(courseId);
            if (UploadFile.TestImage(course.Image))
            {
                filePath = await UploadFile.WriteFile(course.Image);
                if (filePath != "")
                {
                    if (!UploadFile.DeleteFile(currentCourse.ImageUrl))
                        return StatusCode(StatusCodes.Status400BadRequest, "Impossible de supprimer le fichier");
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Type de fichier non valide");

            currentCourse.CourseId = course.CourseId;
            currentCourse.Name = course.Name;
            currentCourse.Description = course.Description;
            currentCourse.Price = course.Price;
            currentCourse.Rate = course.Rate;
            currentCourse.ImageUrl = filePath;
            currentCourse.VideoUrl = course.VideoUrl;
            currentCourse.Languages = null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!CourseExists(courseId))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }

            //suppression des langues attachées à ce course
            var courseLanguageData = await _context.CourseLanguage.Where(c => c.CourseId == course.CourseId).ToListAsync();
            foreach (CourseLanguage cl in courseLanguageData)
            {
                _context.CourseLanguage.Remove(cl);
            }


            try
            {
                //ajout des langues
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }


            //ajout des langues pour le course. on part du principe que les langues existent dans la table langue
            foreach (Language newLanguage in languages)
            {
                CourseLanguage cl = new CourseLanguage() { CourseId = course.CourseId, LanguageId = newLanguage.LanguageId };
                await _context.CourseLanguage.AddAsync(cl);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            //retourne 204
            return NoContent();
        }



        /// <summary>
        /// Créer un nouveau course
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="course">objet course</param>
        /// <returns>Retourne un Course</returns>

        // POST: api/Course/postcourse
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse([FromForm] Course course)
        {
            List<Language> languages = (List<Language>)course.Languages;

            course.Languages = null;

            string filePath = string.Empty;

            if (UploadFile.TestImage(course.Image))
            {
                filePath = await UploadFile.WriteFile(course.Image);
                if (filePath != string.Empty)
                    course.ImageUrl = filePath;
                else
                    return StatusCode(StatusCodes.Status202Accepted, "Impossible de sauvegarder le fichier");
            }
            else
            {
                return StatusCode(StatusCodes.Status203NonAuthoritative, "Type de fichier non valide ou fichier non présent");
            }

            try
            {
                await _context.Course.AddAsync(course);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            //ajout des langues pour le course. on part du principe que les langues existent dans la table langue

            if (languages != null)
            {
                foreach (Language newLanguage in languages)
                {
                    CourseLanguage cl = new CourseLanguage() { CourseId = course.CourseId, LanguageId = newLanguage.LanguageId };
                    await _context.CourseLanguage.AddAsync(cl);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
            //code 201 retourné + l'url pour retrouver la donnée + course
            return CreatedAtAction("GetCours", new { id = course.CourseId }, course);
            //return Ok();
        }



        /// <summary>
        /// Permet de supprimer un course. Supprime en même temps
        /// les achats faits sur ce course et les informations de langue sont supprimés dans courslangue
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="courseId">represente l'identificant d'un course</param>
        /// <returns>l'état de la suppression</returns>

        // DELETE: api/Course/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCours(int courseId)
        {
            var course = await _context.Course.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            try
            {
                UploadFile.DeleteFile(course.ImageUrl);
                _context.Course.Remove(course);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return NoContent();
        }



        private bool CourseExists(int courseId)
        {
            return _context.Course.Any(e => e.CourseId == courseId);
        }

    }
}
