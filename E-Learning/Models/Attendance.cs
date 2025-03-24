using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Ngày có mặt")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string Status => string.IsNullOrEmpty(Description) ? "Vắng" : "Có mặt";
    }
}
