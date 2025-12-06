using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class ChargingSession
{
    public int SessionId { get; set; }

    public int StationId { get; set; }

    public int DriverId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal KwhConsumed { get; set; }

    public decimal Cost { get; set; }

    public virtual SystemUser Driver { get; set; } = null!;

    public virtual ChargingStation Station { get; set; } = null!;
}
