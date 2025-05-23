using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? HireDate { get; set; }

    public int Status { get; set; }

    /// <summary>
    /// Bắt buộc phải có
    /// </summary>
    public string? Email { get; set; }

    public Guid ImageId { get; set; }

    public virtual Aspnetuser? User { get; set; }
}
