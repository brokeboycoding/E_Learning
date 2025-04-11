namespace E_Learning.Models
{
    public class TeacherSchoolClass
    {
        public int TeacherId { get; set; }
        public required Teacher Teacher { get; set; }

        public int SchoolClassId { get; set; }
        public required SchoolClass SchoolClass { get; set; }
    }
}
