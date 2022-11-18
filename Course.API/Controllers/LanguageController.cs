using CourseAPI.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using CourseAPI.Models;
using System.Linq;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly CourseDbContext _context;

        public LanguageController(CourseDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Liste les langues disponibles
        /// Accessible aux personnes authentifiées
        /// </summary>
        /// <returns>Liste de langues</returns>

        [Authorize]
        // GET: api/Language
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguage()
        {
            return await _context.Language.OrderBy(d => d.Name).ToListAsync();
        }



        /// <summary>
        /// Récupère une language
        /// Accessible aux personnes authentifiées
        /// </summary>
        /// <param name="languageId">Identifiant de language</param>
        /// <returns>une language</returns>

        [Authorize]
        // GET: api/Language/5
        [HttpGet("{languageId}")]
        public async Task<ActionResult<Language>> GetLanguage(int languageId)
        {
            var language = await _context.Language.FindAsync(languageId);

            if (language == null)
            {
                return NotFound();
            }

            return language;
        }



        /// <summary>
        /// mise à jour d'une language
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="languageId">Identifiant de language</param>
        /// <param name="language">objet language</param>
        /// <returns>HTTP 204 No Content indique que la requête a réussi HTTP 204 No Content indique que la requête a réussi</returns>

        [Authorize(Roles = "Admin")]
        // PUT: api/Language/5
        [HttpPut("{languageId}")]
        public async Task<IActionResult> PutLanguage(int languageId, Language language)
        {
            if (languageId != language.LanguageId)
            {
                return BadRequest();
            }

            _context.Entry(language).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                if (!LanguageExists(languageId))
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
        /// ajout d'une language
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="language">objet language</param>
        /// <returns>HTTP 204 No Content indique que la requête a réussi</returns>

        [Authorize(Roles = "Admin")]
        // POST: api/Language
        [HttpPost]
        public async Task<ActionResult<Language>> PostLanguage(Language language)
        {
            _context.Language.Add(language);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetLangue", new { id = language.LanguageId }, language);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// supprime une language
        /// la suppression de la language impliquera la suppression
        /// de son référencement dans les cours.
        /// Dans courslangue toutes les lignes associées à cette language sont supprimées
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="languageId">identifiant d'une language</param>
        /// <returns>HTTP 204 No Content indique que la requête a réussi</returns>

        [Authorize(Roles = "Admin")]
        // DELETE: api/Language/5
        [HttpDelete("{languageId}")]
        public async Task<IActionResult> DeleteLangue(int languageId)
        {
            var language = await _context.Language.FindAsync(languageId);
            if (language == null)
            {
                return NotFound();
            }

            //la suppression de la language impliquera la suppression
            //de son référencement dans les cours.
            //Dans courslangue toutes les lignes associées à cette language sont supprimées

            _context.Language.Remove(language);
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



        private bool LanguageExists(int languageId)
        {
            return _context.Language.Any(e => e.LanguageId == languageId);
        }
    }

}

