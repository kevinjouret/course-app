using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseAPI.DataContext;
using CourseAPI.Models;
using CourseAPI.Utils;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseDbContext _context;

        public CoursesController(CourseDbContext context)
        {
            _context = context;
        }

        #region GetCourses

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            List<Course> courses = await _context.Course.ToListAsync();
            if (courses.Count > 0)
                return Ok(courses);
            else
                return NotFound("Pas d'éléments disponibles");
        }

        #endregion

        #region GetCourseById

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course != null)
                return Ok(course);
            else
                return NotFound("Élément introuvable");
        }

        #endregion

        #region Put

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, [FromForm] Course course)
        {
            string filePath;

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            var currentCourse = await _context.Course.FindAsync(id);

            if (UploadFile.TestImage(course.Image))
            {
                filePath = await UploadFile.WriteFile(course.Image);
                if (filePath != null)
                {
                    if (!UploadFile.DeleteFile(currentCourse.ImageUrl)) // Delete old image and write new image
                        return StatusCode(StatusCodes.Status400BadRequest, "Impossible de supprimer le fichier");
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Format de fichier invalide");

            currentCourse.Name = course.Name;
            currentCourse.Description = course.Description;
            currentCourse.Price = course.Price;
            currentCourse.Rate = course.Rate;
            currentCourse.ImageUrl = filePath;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        #endregion

        #region Post

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse([FromForm] Course course) // [FromForm] = Add form (nice for file) in Swagger
        {
            string filePath;

            // Check image extension
            if (UploadFile.TestImage(course.Image))
            {
                filePath = await UploadFile.WriteFile(course.Image); // Write file

                if (filePath != string.Empty)
                    course.ImageUrl = filePath; // Create image url
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Impossible de sauvegarder le fichier");
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Type de fichier invalide ou non présent");


            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            // Return 201 code (OK) + data URL + obj<Course> 
            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        #endregion

        #region Delete

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            UploadFile.DeleteFile(course.ImageUrl); // Delete image also

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region GetCoursesByHighestPrice

        // GET: api/Courses/GetCoursesByHighestRate
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByHighestRate()
        {
            List<Course> courses = await _context.Course.OrderByDescending(c => c.Rate).ToListAsync();
            if (courses.Count > 0)
                return Ok(courses);
            else
                return NotFound("Pas d'éléments disponibles");
        }

        #endregion

        #region GetCoursesByLowestPrice

        // GET: api/Courses/GetCoursesByLowestPrice
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByLowestPrice()
        {
            List<Course> courses = await _context.Course.OrderBy(c => c.Price).ToListAsync();
            if (courses.Count > 0)
                return Ok(courses);
            else
                return NotFound("Pas d'éléments disponibles");
        }

        #endregion

        #region GetUsers

        [HttpGet("[action]")]
        public async Task<IActionResult> RequestTraining(string filter) // Remove filter param if necessary
        {
            // GetAdministatorList
            //var response = await _context.User.Where(u => u.Role == "Administrateur").ToListAsync(); // LINQ
            //var response = from u in _context.User where u.Role == "Administrateur" select u; // SQL

            // GetAdministratorList > Select (concat) Fullname and Email
            //var response = await _context.User.Where(u => u.Role == "Administrateur").Select(a => new { Fullname = a.Firstname + " " + a.Lastname, a.Email }).ToListAsync(); // LINQ (Email = a.Email is not necessary) 
            //var response = from u in _context.User where u.Role == "Administrateur" select new { Fullname = u.Firstname + " " + u.Lastname, Email = u.Email}; // SQL

            // Get Purchase + Price (from Purchase context) and User Fullname (from User context)
            //var response = await _context.Purchase.Join(_context.User, p => p.UserId, u => u.UserId, (p, u) => new { p.PurchaseId, p.Price, Fullname = u.Firstname + " " + u.Lastname}).ToListAsync(); // LINQ
            //var response = from a in _context.Purchase join u in _context.User on a.UserId equals u.UserId select new { a.PurchaseId, a.Price, Fullname = u.Firstname + " " + u.Lastname }; // SQL

            // Get Purchase + Price + User OrderBy Price then by Lastname
            //var response = await _context.Purchase.Join(_context.User, p => p.UserId, u => u.UserId, (p, u) => new {p.Price, Fullname = u.Lastname + " " + u.Firstname} ).OrderBy(op => op.Price).ThenBy(on => on.Fullname).ToListAsync();
            //var response = from p in _context.Purchase join u in _context.User on p.UserId equals u.UserId orderby p.Price orderby u.Lastname select new { p.Price, Fullname = u.Lastname + " " + u.Firstname };

            // Filter city
            //var response = await _context.Address
            //    .Join(_context.User, ad => ad.AddressId, u => u.UserId, (ad, u) => new { u.Lastname, u.Firstname, ad.Street, ad.Zipcode, ad.City })
            //    .Where(a => a.City
            //    .Contains(filter))
            //    .OrderBy(n => n.Lastname)
            //    .ToListAsync();

            // Get User name + address + some Order info
            var response = await _context.Address
                .Join(_context.User, a => a.AddressId, u => u.UserId, (a, u) => new
                {
                    u.Lastname,
                    u.Firstname,
                    a.Street,
                    a.Zipcode,
                    a.City,
                    NbOfOrder = u.Purchases.Count,
                    Orders = u.Purchases.Select(p => new
                    {
                        p.PurchaseId,
                        p.CourseId,
                        p.Price
                    }),
                    Total = u.Purchases.Sum(p => p.Price)
                })
                .Where(a => a.City.Contains(filter))
                .ToListAsync();

            //var response = from a in _context.Address
            //          join u in _context.User
            //          on a.AddressId equals u.UserId
            //          where a.City.Contains(filter)
            //          orderby u.Lastname
            //          select new
            //          {
            //              u.Lastname,
            //              u.Firstname,
            //              a.Street,
            //              a.Zipcode,
            //              a.City,
            //              NbOfOrder = u.Purchases.Count,
            //              Orders = from u in u.Purchases
            //                          select new
            //                          {
            //                              u.PurchaseId,
            //                              u.Price,
            //                              u.CourseId
            //                          },
            //              Total = u.Purchases.Sum(c => c.Price)
            //          };

            return Ok(response);
        }

        #endregion

        #region GetCourses (Simple Pagination)

        // GET: api/Courses
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(string sorting, int pageNum, int pageSize) // Sorting = Name || Price || Rating
        {
            var courseList = from c in _context.Course
                             join cl in _context.CourseLanguage on c.CourseId equals cl.CourseId
                             join l in _context.Language on cl.LanguageId equals l.LanguageId
                             select new
                             {
                                 c.CourseId,
                                 c.Name,
                                 c.Description,
                                 c.Rate,
                                 c.Price,
                                 Language = l.Name
                             };

            if (courseList != null)
                switch (sorting)
                {
                    case "Name":
                        return Ok(courseList.OrderBy(c => c.Name).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
                    case "Price":
                        return Ok(courseList.OrderBy(c => c.Price).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
                    case "Rate":
                        return Ok(courseList.OrderByDescending(c => c.Rate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
                    case "Language":
                        return Ok(courseList.OrderBy(c => c.Language).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
                    default:
                        return Ok(courseList.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
                }
            else
                return NotFound("Pas d'éléments disponibles");
        }

        #endregion

        #region GetCourse (Search)

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Course>>> Search(string keyword)
        {
            if (keyword.Length >= 2)
            {
                var res = from c in _context.Course
                          join cl in _context.CourseLanguage on c.CourseId equals cl.CourseId
                          join l in _context.Language on cl.LanguageId equals l.LanguageId
                          where c.Name.Contains(keyword) || l.Name.Contains(keyword) || c.Description.Contains(keyword)
                          select new
                          {
                              c.CourseId,
                              c.Name,
                              c.Description,
                              c.Price,
                              Language = l.Name
                          };
                
                return Ok(res);

            }
            else
                return BadRequest("La champ de recherche doit contenir au moins 2 charactères");
        }

        #endregion

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
