using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class Tour
{
    public int TourId { get; set; }

    public int? CategoryTourId { get; set; }

    public string? Title { get; set; }

    public string? Duration { get; set; }

    public DateTime? DepartureDate { get; set; }

    public string? Tranfer { get; set; }

    public string? Hotel { get; set; }

    public int? EmptySeat { get; set; }

    public string? PointOfDeparture { get; set; }

    public string? TourType { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? Status { get; set; }

    public string? CategoryTourName { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual CategoryTour? CategoryTour { get; set; }

    public virtual ICollection<DepartureSchedule> DepartureSchedules { get; } = new List<DepartureSchedule>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual ICollection<TourDetail> TourDetails { get; } = new List<TourDetail>();
}
