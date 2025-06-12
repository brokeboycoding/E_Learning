using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Course : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name="Tên khóa học")]
        public string? Name { get; set; }

        [MaxLength(500)]
        [Display(Name="Mô tả khóa học")]
        public string? Description { get; set; }

        [Range(1, 52)]
        [Display(Name="Thời gian khóa học diễn ra")]
        public int Duration { get; set; } // Khoảng thời gian khóa học diễn ra
        [Display(Name = "Khóa học đang hoạt động")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? VideoUrl { get; set; }  // Thuộc tính lưu URL của video
      

       public List<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
    }
}
