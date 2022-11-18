using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CourseAPI.Models
{
    public class Address
    {
        [ForeignKey("User")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "La rue/avenue/boulevard/chemin est obligatoire")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Le code postal est obligatoire")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "La ville est obligatoire")]
        public string City { get; set; }

        [JsonIgnore]
        public User User { get; set; }       
    }
}
