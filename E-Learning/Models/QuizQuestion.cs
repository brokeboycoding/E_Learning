using Microsoft.VisualBasic.FileIO;

namespace E_Learning.Models
{
    public class QuizQuestion : IEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public string? QuestionText { get; set; }

        public ICollection<QuizOption>? Options { get; set; } = new List<QuizOption>();
    }
}
