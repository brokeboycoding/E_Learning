namespace E_Learning.Models
{
    public class CourseViewModel
    {
        public List<Module> Modules { get; set; } = new();
        public Lesson? CurrentLesson { get; set; }
        public Course? CurrentCourse { get; set; }
        public List<LessonNote> Notes { get; set; } = new List<LessonNote>();
        public LessonNote NewNote { get; set; } = new();
    }
}
