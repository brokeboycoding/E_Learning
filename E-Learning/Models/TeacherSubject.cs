namespace E_Learning.Models
{
    public class TeacherSubject
    {
        public int TeacherId { get; set; }
        public required Teacher Teacher { get; set; }

        public int SubjectId { get; set; }
        public required Subject Subject { get; set; }
    }
}
