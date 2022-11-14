using CourseAPI.Utils;
using System.ComponentModel.DataAnnotations;

namespace CourseAPI.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; }

        [MinLength (10, ErrorMessage = "La description doit contenir au moins 10 caractères")]
        public string Description { get; set; }

        // I know Range() is better but it's just a CustomValidation's example
        [CustomValidationModel(10)]
        public int Rate { get; set; }
    }
}
