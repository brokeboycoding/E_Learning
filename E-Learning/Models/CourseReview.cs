using E_Learning.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class CourseReview
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // 1 - 5 sao

        public string Comment { get; set; }
        [Display(Name="Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
