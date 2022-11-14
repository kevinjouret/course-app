using CourseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        #region CourseList

        private static List<Course> CourseList = new()
        {
            new Course()
            {
                Id = 0,
                Name = "Le JavaScript pour les nuls",
                Description = "Apprendre le JavaScript depuis le début.",
                Rate = 8
            },

            new Course()
            {
                Id = 1,
                Name = "Le C# pour les nuls",
                Description = "Apprendre le C# depuis le début.",
                Rate = 9
            },

            new Course()
            {
                Id = 2,
                Name = "HTML5 pour les nuls",
                Description = "Apprendre HTML5 depuis le début.",
                Rate = 7
            },
        };

        #endregion


        #region GetAll

        [HttpGet]
        public IActionResult GetAll()
        {
            // check if list isn't empty
            if (CourseList.Count > 0)
                return Ok(CourseList);
            return NotFound("Pas d'éléments disponibles");
        }

        #endregion

        #region Post

        [HttpPost]
        public IActionResult Post([FromBody] Course course)
        {
            int position = CourseList.FindIndex(p => p.Id == course.Id);

            // Check if item id already exist
            if (position != -1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Identifiant (id) manquant ou déjà existant");
            }
            else
            {
                CourseList.Add(course);
                return Created("", "Élement créé avec succès");
            }
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int position = CourseList.FindIndex(p => p.Id == id);

            if (position != -1)
            {
                CourseList.RemoveAt(position);
                return Ok("Élément supprimé avec succès");
            }
            else
                return NotFound("Élément introuvable");

        }

        #endregion

        #region Put

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Course course)
        {
            int currentPosition = CourseList.FindIndex(p => p.Id == id);

            if (currentPosition != -1)
            {
                int newPosition = CourseList.FindIndex(p => p.Id == course.Id);

                if (currentPosition != -1 && newPosition != -1)
                {
                    CourseList[currentPosition] = course;
                    return Ok("Élément modifié avec succès");
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Modification impossible. Id déjà utilisé");
            }
            else
                return StatusCode(StatusCodes.Status404NotFound, "Élément introuvable");
        }

        #endregion

        #region GetById

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Course course = CourseList[id];
                return Ok(course);
            }
            catch (Exception)
            {
                return NotFound("Élément introuvable");
                throw;
            }
        }

        #endregion
    }
}
