using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Role { get; set; }

    public string? Desciption { get; set; }

    public byte? Gender { get; set; }

    public string? Salt { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();
}
