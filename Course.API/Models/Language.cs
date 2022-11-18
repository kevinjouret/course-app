using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourseAPI.Models
{
    public class Language
    {
        public int LanguageId { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Le nom de la langue doit contenir au moins 5 caractères")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Course> Courses { get; set; } // NN
    }
}
