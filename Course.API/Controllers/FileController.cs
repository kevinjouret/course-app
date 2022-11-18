using CourseAPI.DataContext;
using CourseAPI.Models;
using CourseAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly CourseDbContext _context;

        public FileController(CourseDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Upload un fichier
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="course">objet course</param>
        /// <returns>Retourne 201 si réussite</returns>

        // POST: api/file
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> PostImage([FromForm] Course course)
        {
            string filePath = string.Empty;

            if (UploadFile.TestImage(course.Image))
            {
                filePath = await UploadFile.WriteFile(course.Image);
                if (filePath != "")
                    course.ImageUrl = filePath;
                else
                    StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Type de fichier non valide ou fichier non présent");
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
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
