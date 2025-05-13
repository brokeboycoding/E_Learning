namespace E_Learning.Models
{
    public class CourseViewModel
    {
        public List<Module> Modules { get; set; } = new();
        public Lesson? CurrentLesson { get; set; }
    }
}
