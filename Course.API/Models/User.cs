using System.Collections.Generic;

namespace CourseAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public Address Address { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
