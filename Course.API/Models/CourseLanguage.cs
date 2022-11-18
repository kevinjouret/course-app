namespace CourseAPI.Models
{
    public class CourseLanguage
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
