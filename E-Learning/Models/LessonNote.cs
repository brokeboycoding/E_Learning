namespace E_Learning.Models
{
    public class LessonNote : IEntity
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int LessonId { get; set; }

        public double Timestamp { get; set; } // Thời điểm trong video
        public string Content { get; set; }

        public Lesson? Lesson { get; set; }

        public User? User { get; set; }
    }
}
