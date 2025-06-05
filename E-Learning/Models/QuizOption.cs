namespace E_Learning.Models
{
    public class QuizOption : IEntity
    {
        public int Id { get; set; }
        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }
    }
}
