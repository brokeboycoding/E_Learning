namespace E_Learning.Models
{
    public class LessonProgress : IEntity
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int LessonId { get; set; }

        public bool IsCompleted { get; set; }

        public Lesson? Lesson { get; set; }
    }
}
