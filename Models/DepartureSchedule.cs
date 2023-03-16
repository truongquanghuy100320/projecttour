using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class DepartureSchedule
{
    public int DepartureScheduleId { get; set; }

    public DateTime? DepartureDate { get; set; }

    public string? Standard { get; set; }

    public double? Price { get; set; }

    public int? EmptySeat { get; set; }

    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }
}
