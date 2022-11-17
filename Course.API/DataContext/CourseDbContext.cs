using CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CourseAPI.DataContext
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {

        }

        public DbSet<Course> Course { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<CourseLanguage> CourseLanguage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Course

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, Name = "Xamarin", Description = "Créer une application multiplateforme avec Xamarin", Rate = 8, Price = 59, ImageUrl = "wwwroot\\images\\xamarin.PNG" },
                new Course { CourseId = 2, Name = "C#", Description = "Apprendre et maitriser le langage C#", Rate = 9, Price = 54, ImageUrl = "wwwroot\\images\\csharp.PNG" },
                new Course { CourseId = 3, Name = "HTML 5", Description = "HTML 5 pour les nuls", Rate = 7, Price = 35, ImageUrl = "wwwroot\\images\\html5.PNG" }
            );

            #endregion

            #region Seed Language

            modelBuilder.Entity<Language>().HasData(
                new Language { LanguageId = 1, Name = "Français" },
                new Language { LanguageId = 2, Name = "Italien" },
                new Language { LanguageId = 3, Name = "Chinois" },
                new Language { LanguageId = 4, Name = "Anglais" }
                );

            #endregion

            #region Seed User

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Firstname = "Dupond", Lastname = "Marc", Email = "dupond@test.fr", Password = "123", Role = "Utilisateur" },
                new User { UserId = 2, Firstname = "Martin", Lastname = "Jean", Email = "martin@test.fr", Password = "123", Role = "Utilisateur" },
                new User { UserId = 3, Firstname = "Henri", Lastname = "Paul", Email = "henri@test.fr", Password = "123", Role = "Utilisateur" },
                new User { UserId = 4, Firstname = "Dubail", Lastname = "Rose", Email = "dubail@test.fr", Password = "123", Role = "Administrateur" }
                );

            #endregion

            #region Seed Address

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, Street = "Les oiseaux sauvages", Zipcode = 13010, City = "Marseille" },
                new Address { AddressId = 2, Street = "Les jas chantants", Zipcode = 75000, City = "Paris" },
                new Address { AddressId = 3, Street = "Les grands cèdres", Zipcode = 06000, City = "Marseille" }
                );

            #endregion

            #region Seed Purchase

            modelBuilder.Entity<Purchase>().HasData(
                new Purchase { PurchaseId = 1, Price = 60, CourseId = 1, UserId = 1 },
                new Purchase { PurchaseId = 2, Price = 80, CourseId = 2, UserId = 3 },
                new Purchase { PurchaseId = 3, Price = 55, CourseId = 3, UserId = 2 }
                );

            #endregion

            #region Seed CourseLanguage

            // Create primary key for this table before
            modelBuilder.Entity<CourseLanguage>()
           .HasKey(o => new { o.CourseId, o.LanguageId });

            modelBuilder.Entity<CourseLanguage>().HasData(
                new CourseLanguage { CourseId = 1, LanguageId = 1 },
                new CourseLanguage { CourseId = 1, LanguageId = 3 },
                new CourseLanguage { CourseId = 1, LanguageId = 4 },
                new CourseLanguage { CourseId = 2, LanguageId = 1 },
                new CourseLanguage { CourseId = 2, LanguageId = 2 },
                new CourseLanguage { CourseId = 2, LanguageId = 4 },
                new CourseLanguage { CourseId = 3, LanguageId = 1 },
                new CourseLanguage { CourseId = 3, LanguageId = 2 },
                new CourseLanguage { CourseId = 3, LanguageId = 4 }
            );

            #endregion
        }
    }
}
