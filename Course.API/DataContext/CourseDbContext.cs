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
            modelBuilder.Entity<Language>()
            .HasMany(c => c.Courses)
            .WithMany(l => l.Languages)
            .UsingEntity<CourseLanguage>(
                cl => cl
                    .HasOne(cl => cl.Course)
                    .WithMany()
                    .HasForeignKey("CourseId"),
                cl => cl
                    .HasOne(cl => cl.Language)
                    .WithMany()
                    .HasForeignKey("LanguageId"))
            .ToTable("CoursLangue")
            .HasKey(cl => new { cl.CourseId, cl.LanguageId });

            #region Seed Course

            modelBuilder.Entity<Course>().HasData(
                 new Course { CourseId = 1, Name = "Xamarin Forms: créer des applications mobiles natives avec C #", Description = "Apprenez les bases de la création d'applications mobiles avec Xamarin Forms.", Rate = 15, ImageUrl = "wwwroot\\images\\1.PNG", Price = 50, VideoUrl = "https://www.youtube.com/watch?v=93ZU6j59wL4" },
                 new Course { CourseId = 2, Name = "Faites des présentations parfaites avec PowerPoint", Description = "Découvre les fonctionnalités et les techniques pour faire des présentations qui mobilisent tout le monde", Rate = 18, ImageUrl = "wwwroot\\images\\2.PNG", Price = 70, VideoUrl = "https://www.youtube.com/watch?v=Zk_s2Xzjikc" },
                 new Course { CourseId = 3, Name = "Découvrir les fractions CM1 - CM2 - Cycle 3 - Maths", Description = "Permet de découvrir la notion de fraction simple. Elle est essentiellement destinée aux élèves de CM1, mais peut-être utilisée comme un rappel pour les élèves de CM2. Il s'agit de découvrir les fractions à partir d'une situation de la vie courante.", Rate = 10, ImageUrl = "wwwroot\\images\\3.PNG", Price = 45, VideoUrl = "https://www.youtube.com/watch?v=4XsxxcuHUzg" },
                 new Course { CourseId = 4, Name = "Business intelligence : Comprendre Business Intelligence ou informatique décisionnelle", Description = "Cette formation vous permet de comprendre et d'identifier la business intelligence appelée aussi informatique décisionnelle", Rate = 16, ImageUrl = "wwwroot\\images\\4.PNG", Price = 75, VideoUrl = "https://www.youtube.com/watch?v=s2lq_Jxf8i8" },
                 new Course { CourseId = 5, Name = "PROSPECTION COMMERCIALE : Comment Trouver Des Clients BtoB et BtoC ?", Description = "Obtenir de nouveaux prospects (des clients potentiels) c’est indispensable pour la survie de votre entreprise. La bonne nouvelle : différentes méthodes de prospection s’offrent à vous ! Reste à savoir lesquelles sont les plus adaptées à vous.Voyons 10 stratégies de prospection commerciale b2b et b2c efficace et rentable.", Rate = 17, ImageUrl = "wwwroot\\images\\5.PNG", Price = 82, VideoUrl = "https://www.youtube.com/watch?v=Xe1lnf-DLTo" },
                 new Course { CourseId = 6, Name = "Formation marketing / cours marketing complet", Description = "Cours marketing / formation marketing gratuit / tutoriel marketing complet avec le marketing digital, le marketing stratégique et le eCommerce.", Rate = 13, ImageUrl = "wwwroot\\images\\6.PNG", Price = 64, VideoUrl = "https://www.youtube.com/watch?v=lIvl1syEKfs" },
                 new Course { CourseId = 7, Name = "Marketing digital / Cours marketing digital", Description = "Ce cours de marketing digital vous donne les étapes clés du webmarketing.", Rate = 13, ImageUrl = "wwwroot\\images\\7.PNG", Price = 45, VideoUrl = "https://www.youtube.com/watch?v=X4FVO_nZn5w" },
                 new Course { CourseId = 8, Name = "L'art de la communication : Les 3 préférences comportementales de JUNG.", Description = "L'art de la communication : Les 3 préférences comportementales de JUNG.", Rate = 16, ImageUrl = "wwwroot\\images\\8.PNG", Price = 63, VideoUrl = "https://www.youtube.com/watch?v=CTQblWIHRTc" },
                 new Course { CourseId = 9, Name = "Comment créer une application mobile ?", Description = "Faire ses premiers pas dans le développement d'application mobile.", Rate = 17, ImageUrl = "wwwroot\\images\\9.PNG", Price = 74, VideoUrl = "https://www.youtube.com/watch?v=xSNNki7YNKk" },
                 new Course { CourseId = 10, Name = "La Formation Complète Python", Description = "Apprendre Python gratuitement, c'est possible grâce à ces 7h de formation. je vous explique tout ce que vous avez besoin de savoir pour créer vos premiers scripts avec Python.", Rate = 16, ImageUrl = "wwwroot\\images\\10.PNG", Price = 59, VideoUrl = "https://www.youtube.com/watch?v=LamjAFnybo0" },
                 new Course { CourseId = 11, Name = "Xamarin Tutorial What is Xamarin? | Xamarin Tutorial Introduction Overview for Beginners", Description = "Xamarin is Microsoft's framework for creating mobile apps. In this video get an overview of all the various piecesof Xamarin and how they enable you to create cross platform mobile applications.", Rate = 15, ImageUrl = "images/11.PNG", Price = 82, VideoUrl = "https://www.youtube.com/watch?v=ZRvJX8k-wZE&list=PLNcJs0ZTbcSoNQCA3SWDLEYlrofE52dji" },
                 new Course { CourseId = 12, Name = "Cours de Français Gratuit pour Débutants", Description = "Cours de français pour débutants.Comment saluer en français et quelques principes fondamentaux du français.", Rate = 14, ImageUrl = "wwwroot\\images\\12.PNG", Price = 40, VideoUrl = "https://www.youtube.com/watch?v=2sCXhPefmz8" },
                 new Course { CourseId = 13, Name = "FORMATION DEEP LEARNING COMPLETE", Description = "cette formation sur le Deep Learning vous apprendra à développer des réseaux de neurones artificiels, en voyant tous les détails mathématiques qui se cachent derrière l'intelligence artificielle. Nous apprendrons à développer des modeles de Deep Learning avec Numpy, mais aussi avec Keras et Tensorflow.", Rate = 18, ImageUrl = "wwwroot\\images\\13.PNG", Price = 98, VideoUrl = "https://www.youtube.com/watch?v=XUFLq6dKQok" },
                 new Course { CourseId = 14, Name = "La Base De La Mécanique Pour Les Débutants. Moteur/Pneus/Niveaux Liquides", Description = "Apprenez à, entretenir votre véhicule en maitrisant les bases de la mécanique. Surveillez votre moteur, controlez vos niveaux de liquide de frein et d'eau et roulez bien chaussé.", Rate = 13, ImageUrl = "wwwroot\\images\\14.PNG", Price = 38, VideoUrl = "https://www.youtube.com/watch?v=1lpkF3Sx9qI" },
                 new Course { CourseId = 15, Name = "Plan de Formation-les 8 étapes essentielles - RH", Description = "Le plan de formation d'une entreprise rassemble toutes les actions de formation destinées à ses salariés que ce soit pour : 1/les aider à s'adapter à leur poste de travail en cas d'évolution ; 2/ou bien pour développer leurs compétences.", Rate = 17, ImageUrl = "wwwroot\\images\\15.PNG", Price = 12, VideoUrl = "https://www.youtube.com/watch?v=x2UunFSGyf0" }
                 );


            #endregion

            #region Seed Language

            modelBuilder.Entity<Language>().HasData(
                new Language { LanguageId = 1, Name = "Français" },
                new Language { LanguageId = 2, Name = "Italien" },
                new Language { LanguageId = 3, Name = "Chinois" },
                new Language { LanguageId = 4, Name = "Anglais" },
                new Language { LanguageId = 5, Name = "Espagnol" },
                new Language { LanguageId = 6, Name = "Allemand" }
                );

            #endregion

            #region Seed User

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Lastname = "Dupond", Firstname = "Marc", Email = "dupond@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 2, Lastname = "Martin", Firstname = "Jean", Email = "martin@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 3, Lastname = "Henri", Firstname = "Paul", Email = "henri@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 4, Lastname = "Dubail", Firstname = "Rose", Email = "dubail@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "Admin" },
                new User { UserId = 5, Lastname = "Petit", Firstname = "Valérie", Email = "Petit@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 6, Lastname = "Klein", Firstname = "Robet", Email = "Klein@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 7, Lastname = "Muller", Firstname = "Samia", Email = "Muller@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" },
                new User { UserId = 8, Lastname = "Bahou", Firstname = "Amine", Email = "bahou@test.fr", Password = "$2a$11$MwV97GykjjNH0tcgskauROXi2F4bWxr84/OmIjVnkaMpPBsdt0zv.", Role = "User" }
                );

            #endregion

            #region Seed Address

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, Street = "Les oiseaux sauvages", Zipcode = "13010", City = "Marseille" },
                new Address { AddressId = 2, Street = "Les jas chantants", Zipcode = "75000", City = "Paris" },
                new Address { AddressId = 3, Street = "Les grands cèdres", Zipcode = "06000", City = "Nice" },
                new Address { AddressId = 4, Street = "Des mouettes chantantes", Zipcode = "06100", City = "Nice" },
                new Address { AddressId = 5, Street = "Chemin des roses", Zipcode = "83430", City = "Saint-Mandrier" },
                new Address { AddressId = 6, Street = "Marc Baron", Zipcode = "24210", City = "Mauzac" },
                new Address { AddressId = 7, Street = "Eugène Laroche", Zipcode = "24210", City = "La bachellerie" },
                new Address { AddressId = 8, Street = "Emile Zola", Zipcode = "3100", City = "Blagnac" }
                );

            #endregion

            #region Seed Purchase

            modelBuilder.Entity<Purchase>().HasData(
                new Purchase { PurchaseId = 1, Price = 65, CourseId = 1, UserId = 1, OrderDate = Convert.ToDateTime("2021-01-12") },
                new Purchase { PurchaseId = 2, Price = 91, CourseId = 2, UserId = 3, OrderDate = Convert.ToDateTime("2021-04-08") },
                new Purchase { PurchaseId = 3, Price = 58.5F, CourseId = 3, UserId = 2, OrderDate = Convert.ToDateTime("2021-04-07") },
                new Purchase { PurchaseId = 4, Price = 52, CourseId = 12, UserId = 3, OrderDate = Convert.ToDateTime("2021-05-08") },
                new Purchase { PurchaseId = 5, Price = 76.7F, CourseId = 10, UserId = 2, OrderDate = Convert.ToDateTime("2021-04-06") },
                new Purchase { PurchaseId = 6, Price = 58.5F, CourseId = 7, UserId = 5, OrderDate = Convert.ToDateTime("2021-07-25") },
                new Purchase { PurchaseId = 7, Price = 81.9F, CourseId = 8, UserId = 5, OrderDate = Convert.ToDateTime("2021-04-22") },
                new Purchase { PurchaseId = 8, Price = 83.2F, CourseId = 6, UserId = 6, OrderDate = Convert.ToDateTime("2021-03-22") },
                new Purchase { PurchaseId = 9, Price = 49.4F, CourseId = 14, UserId = 6, OrderDate = Convert.ToDateTime("2021-07-14") },
                new Purchase { PurchaseId = 10, Price = 15.6F, CourseId = 15, UserId = 8, OrderDate = Convert.ToDateTime("2021-05-09") },
                new Purchase { PurchaseId = 11, Price = 127.4F, CourseId = 13, UserId = 4, OrderDate = Convert.ToDateTime("2021-08-15") },
                new Purchase { PurchaseId = 12, Price = 58.5F, CourseId = 7, UserId = 7, OrderDate = Convert.ToDateTime("2021-02-15") },
                new Purchase { PurchaseId = 13, Price = 96.2F, CourseId = 9, UserId = 7, OrderDate = Convert.ToDateTime("2021-03-03") },
                new Purchase { PurchaseId = 14, Price = 15.6F, CourseId = 15, UserId = 1, OrderDate = Convert.ToDateTime("2021-06-12") },
                new Purchase { PurchaseId = 15, Price = 52, CourseId = 12, UserId = 8, OrderDate = Convert.ToDateTime("2021-04-02") }
                );

            #endregion

            #region Seed CourseLanguage

            modelBuilder.Entity<CourseLanguage>().HasData(
                new CourseLanguage { CourseId = 1, LanguageId = 1 },
                new CourseLanguage { CourseId = 1, LanguageId = 3 },
                new CourseLanguage { CourseId = 1, LanguageId = 4 },
                new CourseLanguage { CourseId = 2, LanguageId = 1 },
                new CourseLanguage { CourseId = 2, LanguageId = 2 },
                new CourseLanguage { CourseId = 2, LanguageId = 4 },
                new CourseLanguage { CourseId = 3, LanguageId = 1 },
                new CourseLanguage { CourseId = 3, LanguageId = 2 },
                new CourseLanguage { CourseId = 3, LanguageId = 4 },
                new CourseLanguage { CourseId = 4, LanguageId = 5 },
                new CourseLanguage { CourseId = 4, LanguageId = 6 },
                new CourseLanguage { CourseId = 4, LanguageId = 1 },
                new CourseLanguage { CourseId = 4, LanguageId = 4 },
                new CourseLanguage { CourseId = 5, LanguageId = 1 },
                new CourseLanguage { CourseId = 5, LanguageId = 4 },
                new CourseLanguage { CourseId = 6, LanguageId = 2 },
                new CourseLanguage { CourseId = 6, LanguageId = 3 },
                new CourseLanguage { CourseId = 6, LanguageId = 4 },
                new CourseLanguage { CourseId = 6, LanguageId = 5 },
                new CourseLanguage { CourseId = 7, LanguageId = 1 },
                new CourseLanguage { CourseId = 7, LanguageId = 5 },
                new CourseLanguage { CourseId = 7, LanguageId = 4 },
                new CourseLanguage { CourseId = 8, LanguageId = 2 },
                new CourseLanguage { CourseId = 8, LanguageId = 4 },
                new CourseLanguage { CourseId = 8, LanguageId = 6 },
                new CourseLanguage { CourseId = 9, LanguageId = 3 },
                new CourseLanguage { CourseId = 9, LanguageId = 4 },
                new CourseLanguage { CourseId = 9, LanguageId = 5 },
                new CourseLanguage { CourseId = 10, LanguageId = 4 },
                new CourseLanguage { CourseId = 10, LanguageId = 1 },
                new CourseLanguage { CourseId = 11, LanguageId = 6 },
                new CourseLanguage { CourseId = 11, LanguageId = 1 },
                new CourseLanguage { CourseId = 11, LanguageId = 4 },
                new CourseLanguage { CourseId = 12, LanguageId = 1 },
                new CourseLanguage { CourseId = 12, LanguageId = 4 },
                new CourseLanguage { CourseId = 13, LanguageId = 1 },
                new CourseLanguage { CourseId = 13, LanguageId = 3 },
                new CourseLanguage { CourseId = 13, LanguageId = 6 },
                new CourseLanguage { CourseId = 13, LanguageId = 2 },
                new CourseLanguage { CourseId = 13, LanguageId = 5 },
                new CourseLanguage { CourseId = 14, LanguageId = 4 },
                new CourseLanguage { CourseId = 14, LanguageId = 5 },
                new CourseLanguage { CourseId = 14, LanguageId = 6 },
                new CourseLanguage { CourseId = 14, LanguageId = 3 },
                new CourseLanguage { CourseId = 15, LanguageId = 4 },
                new CourseLanguage { CourseId = 15, LanguageId = 1 },
                new CourseLanguage { CourseId = 15, LanguageId = 5 }
                );

            #endregion
        }
    }
}
