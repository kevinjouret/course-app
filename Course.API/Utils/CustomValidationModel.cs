using CourseAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CourseAPI.Utils
{
    public class CustomValidationModel : ValidationAttribute
    {
        private readonly int _limit;

        public CustomValidationModel(int limit)
        {
            _limit = limit;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Course course = (Course)validationContext.ObjectInstance;
            if (course.Rate < 0)
                return new ValidationResult("La note doit être supérieure à 0");
            else
            {
                if (course.Rate < _limit)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("La note doit être inférieure à 10");
            }
        }
    }
}
