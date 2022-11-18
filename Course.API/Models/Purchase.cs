using System;
using System.ComponentModel.DataAnnotations;

namespace CourseAPI.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        public float Price { get; set; }

        [Required(ErrorMessage = "La date est obligatoire")]
        public DateTime OrderDate { get; set; }

        public virtual int UserId { get; set; } // Required to seed data
        public virtual int CourseId { get; set; } // Required to seed data
    }
}
