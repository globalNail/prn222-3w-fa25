using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class LionProfile
{
    public int LionProfileId { get; set; }

    public int LionTypeId { get; set; }

    public string LionName { get; set; } = null!;

    public double Weight { get; set; }

    public string Characteristics { get; set; } = null!;

    public string Warning { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual LionType LionType { get; set; } = null!;
}
