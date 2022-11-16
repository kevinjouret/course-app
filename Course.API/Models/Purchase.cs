namespace CourseAPI.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public float Price { get; set; } 
        
        public virtual int UserId { get; set; } // Required to seed data
        public virtual int CourseId { get; set; } // Required to seed data
    }
}
