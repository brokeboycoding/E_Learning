using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Certificate : IEntity
    {
        public int Id { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

       
        public double Value { get; set; }

        public Grade Grade { get; set; }

        [Required]
        public string StudentId { get; set; }

        public User Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
