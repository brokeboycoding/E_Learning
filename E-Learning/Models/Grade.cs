using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Grade
{
    public int Id { get; set; }

    public double Value { get; set; }

    public int StudentId { get; set; }

    public DateTime EvaluationDate { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual Student Student { get; set; } = null!;
}
