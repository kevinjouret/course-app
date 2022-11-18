using CourseAPI.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAPI.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required (ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        [MinLength (10, ErrorMessage = "La description doit contenir au moins 10 caractères")]
        public string Description { get; set; }

        // I know Range() is better but it's just a CustomValidation's example
        [CustomValidationModel(10)]
        public int Rate { get; set; }

        [Range(0, 500, ErrorMessage = "Le prix doit être en 0 et 500€")]
        public float Price { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "L'url d'une video est obligatoire")]
        [Url(ErrorMessage = "L'adresse de la vidéo doit être valide")]
        public string VideoUrl { get; set; }


        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Language> Languages { get; set; } // NN
    }
}
