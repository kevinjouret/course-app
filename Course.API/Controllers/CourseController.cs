using CourseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
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

        #region GetAll

        [HttpGet]
        public IEnumerable<Course> GetAll()
        {
            return CourseList;
        }

        #endregion

        #region Post

        [HttpPost]
        public void Post([FromBody] Course course)
        {
            CourseList.Add(course);
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            int position = CourseList.FindIndex(p => p.Id == id);

            if (position != -1)
            {
                CourseList.RemoveAt(id);
            }
        }

        #endregion

        #region Put

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Course course)
        {
            int position = CourseList.FindIndex(p => p.Id == id);

            if (position != -1)
            {
                CourseList[position] = course;
            }
        }

        #endregion

        #region GetById

        [HttpGet("{id}")]
        public Course GetById(int id)
        {
            Course course = CourseList.Find(p => p.Id == id);
            return course;
        }

        #endregion
    }
}
