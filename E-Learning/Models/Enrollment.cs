using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Enrollment : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int CourseId { get; set; }

        // Navigation
        public Course Course { get; set; }
        public User User { get; set; }
    }
}
