using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Resource : IEntity
    {
        public int Id { get; set; }

       
        public string FileName { get; set; }

        [Required]
        public string FileUrl { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
