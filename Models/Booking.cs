using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? TourId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerEmail { get; set; }

    public string? ContactNumber { get; set; }

    public int? NumberOfAdults { get; set; }

    public int? NumberOfChildren { get; set; }

    public DateTime? BookingDate { get; set; }

    public double? TotalPrice { get; set; }

    public byte? StatusPayment { get; set; }

    public int? CustomerId { get; set; }

    public string? Description { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual User? Customer { get; set; }

    public virtual Tour? Tour { get; set; }
}
