using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Grade : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "Điểm trong khoảng 0 - 10.")]
        public double Value { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

    
    

        [Display(Name = "Ngày đánh giá")]
        public DateTime EvaluationDate { get; set; } = DateTime.Now;

       
        public string Status => Value >= 4.5 ? "Đậu" : "Rớt";
    }
}
