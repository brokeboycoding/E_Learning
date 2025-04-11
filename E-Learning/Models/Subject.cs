﻿using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Subject : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public int Credits { get; set; }

        public int TotalClasses { get; set; }

   
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

       
        public ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();
    }
}
