using CourseAPI.Utils;
using Microsoft.AspNetCore.Http;
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

        [MinLength (10, ErrorMessage = "La description doit contenir au moins 10 caractères")]
        public string Description { get; set; }

        // I know Range() is better but it's just a CustomValidation's example
        [CustomValidationModel(10)]
        public int Rate { get; set; }

        public float Price { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<CourseLanguage> CourseLanguages { get; set; } // NN
    }
}
