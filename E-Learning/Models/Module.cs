namespace E_Learning.Models
{
    public class Module : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
