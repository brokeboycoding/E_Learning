using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public DateTime IssueDate { get; set; }

    public double Value { get; set; }

    public int? GradeId { get; set; }

    public string StudentId { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Grade? Grade { get; set; }

    public virtual Aspnetuser Student { get; set; } = null!;
}
