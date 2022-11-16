using System.Collections.Generic;

namespace CourseAPI.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }

        public ICollection<CourseLanguage> CourseLanguages { get; set; } // NN
    }
}
