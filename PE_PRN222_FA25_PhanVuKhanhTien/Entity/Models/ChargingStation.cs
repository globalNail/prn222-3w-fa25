using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class ChargingStation
{
    public int StationId { get; set; }

    public string StationName { get; set; } = null!;

    public string? Location { get; set; }

    public decimal? MaxPower { get; set; }

    public virtual ICollection<ChargingSession> ChargingSessions { get; set; } = new List<ChargingSession>();
}
