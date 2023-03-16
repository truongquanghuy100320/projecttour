using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public string? PhoneNumber { get; set; }
}
