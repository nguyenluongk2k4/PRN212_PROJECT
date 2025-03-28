using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int? Rate { get; set; }

    public string? Content { get; set; }

    public DateTime? TimeFeedback { get; set; }
}
