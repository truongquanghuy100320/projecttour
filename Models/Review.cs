using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? TourId { get; set; }

    public string? FullName { get; set; }

    public string? Rating { get; set; }

    public string? Comment { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? CustomerId { get; set; }

    public virtual User? Customer { get; set; }

    public virtual Tour? Tour { get; set; }
}
