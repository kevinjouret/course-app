using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [MinLength(3, ErrorMessage = "Le mot de passe doit contenir au moins 3 caractères")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Le rôle est obligatoire")]
        public string Role { get; set; }

        public Address Address { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
