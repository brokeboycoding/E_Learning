using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class QuizCreateViewModel
    {
        public int LessonId { get; set; }

      
        public string QuestionText { get; set; }

        public List<QuizOptionViewModel> Options { get; set; } = new();
        
        
    }

    public class QuizOptionViewModel
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }
    }

}
