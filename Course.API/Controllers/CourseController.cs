using CourseAPI.Models;
using CourseAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    // Add XML format
    [Produces("application/json", "application/xml", Type = typeof(List<string>))]
    [ApiController]
    public class CourseController : ControllerBase
    {
        #region CourseList

        private static List<Course> CourseList = new()
        {
            new Course()
            {
                CourseId = 0,
                Name = "Le JavaScript pour les nuls",
                Description = "Apprendre le JavaScript depuis le début.",
                Rate = 8
            },

            new Course()
            {
                CourseId = 1,
                Name = "Le C# pour les nuls",
                Description = "Apprendre le C# depuis le début.",
                Rate = 9
            },

            new Course()
            {
                CourseId = 2,
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

        #region GetCoursesByRate

        [HttpGet("[action]")] // [HttpGet("GetCoursesByRate")] >> Example of using action attribute
        public IActionResult GetCoursesByRate()
        {
            var courseList = CourseList.OrderBy(c => c.Rate);
            return Ok(courseList);
        }

        #endregion

        #region GetBestCourse

        [HttpGet("[action]")] // [HttpGet("GetBestCourse")] >> Example of using action attribute
        public IActionResult GetBestCourse()
        {
            var bestCourse = CourseList.OrderBy(c => c.Rate).FirstOrDefault();
            return Ok(bestCourse);
        }

        #endregion

        #region GetCourseName

        [HttpGet("[action]/{id}")] // [HttpGet("GetCourseName/{id}")] >> Example of using action attribute
        public IActionResult GetCourseName(int id)
        {
            var courseName = CourseList.Where(c => c.CourseId == id).Select(cn => cn.Name);
            return Ok(courseName);
        }

        #endregion

        #region GetCourseDescription

        [Route("[action]/{id}")] // [HttpGet("GetCourseDescription/{id}")] >> Example of using action attribute
        [HttpGet]
        public IActionResult GetCourseDescription(int id)
        {
            var courseDescription = CourseList.Where(c => c.CourseId == id).Select(cd => cd.Description);
            return Ok(courseDescription);
        }

        #endregion

        #region Post

        //[HttpPost]
        //public IActionResult Post([FromForm] Course course)
        //{
        //    string filePath;
        //    int position = CourseList.FindIndex(p => p.CourseId == course.CourseId);

        //    // Check if item id already exist
        //    if (position != -1)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, "Identifiant (id) manquant ou déjà existant");
        //    }
        //    else
        //    {
        //        // Check image extension
        //        if (UploadFile.TestImage(course.Image))
        //        {                   
        //            filePath = UploadFile.WriteFile(course.Image); // Write file

        //            if (filePath != string.Empty)                        
        //                course.ImageUrl = filePath; // Create image url
        //            else
        //                return StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
        //        }
        //        else
        //            return StatusCode(StatusCodes.Status400BadRequest, "Type de fichier invalide ou non présent");

        //        CourseList.Add(course);
        //        return Created("", "Élement créé avec succès");
        //    }
        //}

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int position = CourseList.FindIndex(p => p.CourseId == id);

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

        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromForm] Course course)
        //{
        //    string filePath;
        //    int currentPosition = CourseList.FindIndex(p => p.CourseId == id);

        //    if (currentPosition != -1)
        //    {
        //        int newPosition = CourseList.FindIndex(p => p.CourseId == course.CourseId);

        //        if (currentPosition != -1 && newPosition != -1)
        //        {
        //            if (UploadFile.TestImage(course.Image))
        //            {
        //                filePath = UploadFile.WriteFile(course.Image);
        //                if (filePath != null)
        //                    if (UploadFile.DeleteFile(CourseList[currentPosition].ImageUrl)) // Delete old image and write new image
        //                        course.ImageUrl = filePath;
        //                    else
        //                        return StatusCode(StatusCodes.Status400BadRequest, "Impossible de supprimer le fichier");
        //                else
        //                    return StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
        //            }
        //            else
        //                return StatusCode(StatusCodes.Status400BadRequest, "Format de fichier invalide");

        //            CourseList[currentPosition] = course;
        //            return Ok("Élément modifié avec succès");
        //        }
        //        else
        //            return StatusCode(StatusCodes.Status400BadRequest, "Modification impossible. Id déjà utilisé");
        //    }
        //    else
        //        return StatusCode(StatusCodes.Status404NotFound, "Élément introuvable");
        //}

        #endregion
    }
}
