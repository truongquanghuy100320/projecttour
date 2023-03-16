using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class CategoryTour
{
    public int CategoryTourId { get; set; }

    public string? CategoryTourName { get; set; }

    public virtual ICollection<Tour> Tours { get; } = new List<Tour>();
}
