using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_Learning.Models
{
    public class Lesson : IEntity
    {
    public int Id { get; set; }
    [Display(Name="Tiêu đề bài học")]
    public string Title { get; set; }
    public string? VideoUrl { get; set; }
    [Display(Name ="Mô tả bài học")]
    public string Description { get; set; }
    public string? DocumentUrl {  get; set; }
    public ICollection<QuizQuestion>? QuizQuestions { get; set; }
    public int ModuleId { get; set; }
    [ValidateNever]
    public Module Module { get; set; }
    
    }
}
