namespace E_Learning.Models
{
    public class CourseModuleViewModel
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
           public int Duration { get; set; } // Khoảng thời gian khóa học diễn ra

           public bool IsActive { get; set; } = true;

        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();
        

        public class ModuleViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

           
        }
    }
}
