using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAPI.Models
{
    public class Address
    {
        [ForeignKey("User")]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }

        public User User { get; set; }       
    }
}
