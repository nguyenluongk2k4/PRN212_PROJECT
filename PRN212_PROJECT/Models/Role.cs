using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
