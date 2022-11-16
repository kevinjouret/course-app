using CourseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAPI.DataContext
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {

        }

        public DbSet<Course> Course { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Xamarin", Description = "Créer une application multiplateforme avec Xamarin", Rate = 8, Price = 59, ImageUrl = "wwwroot\\images\\xamarin.PNG" },
                new Course { Id = 2, Name = "C#", Description = "Apprendre et maitriser le langage C#", Rate = 9, Price = 54, ImageUrl = "wwwroot\\images\\csharp.PNG" },
                new Course { Id = 3, Name = "HTML 5", Description = "HTML 5 pour les nuls", Rate = 7, Price = 35, ImageUrl = "wwwroot\\images\\html5.PNG" }
                );
        }
    }
}
