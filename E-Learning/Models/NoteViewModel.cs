namespace E_Learning.Models
{
    public class NoteViewModel
    {
        public int LessonId { get; set; }
        public string? Content { get; set; }
        public TimeSpan Timestamp { get; set; }
    }
}
