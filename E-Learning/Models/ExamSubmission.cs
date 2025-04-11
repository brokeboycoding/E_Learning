using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class ExamSubmission : IEntity
    {
        public int Id { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string ?FileName { get; set; }

        [Required]
        public string ?FileUrl { get; set; }

        [Required]
        public string ?StudentId { get; set; }

        public User ?Student { get; set; }

        public int CourseId { get; set; }

        public Course? Course { get; set; }
    }
}
