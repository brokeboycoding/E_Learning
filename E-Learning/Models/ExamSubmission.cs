using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Examsubmission
{
    public int Id { get; set; }

    public DateTime SubmissionDate { get; set; }

    public string FileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Aspnetuser Student { get; set; } = null!;
}
