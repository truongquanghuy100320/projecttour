using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class TourDetail
{
    public int TourDetailId { get; set; }

    public string? Header { get; set; }

    public string? Tiltle { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }
}
