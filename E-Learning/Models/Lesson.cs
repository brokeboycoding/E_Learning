using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_Learning.Models
{
    public class Lesson : IEntity
    {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? VideoUrl { get; set; }
    public string? Description { get; set; }
    public int ModuleId { get; set; }
    [ValidateNever]
    public Module? Module { get; set; }
    
    }
}
