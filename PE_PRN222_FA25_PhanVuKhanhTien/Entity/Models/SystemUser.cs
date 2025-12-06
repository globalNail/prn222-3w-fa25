using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class SystemUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int UserRole { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<ChargingSession> ChargingSessions { get; set; } = new List<ChargingSession>();
}
