using E_Learning.Data;

namespace E_Learning.Models
{
    public class Discussion : IEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Lesson? Lesson { get; set; }
        public User? User { get; set; }
    }
}
