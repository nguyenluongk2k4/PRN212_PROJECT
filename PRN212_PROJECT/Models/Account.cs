using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? Fullname { get; set; }
}
