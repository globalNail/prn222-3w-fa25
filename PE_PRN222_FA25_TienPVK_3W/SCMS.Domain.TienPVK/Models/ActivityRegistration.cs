using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class ActivityRegistration
{
    public int RegistrationId { get; set; }

    public int ActivityId { get; set; }

    public int MemberId { get; set; }

    public DateTime RegisteredAt { get; set; }

    public bool IsCheckedIn { get; set; }

    public DateTime? CheckedInAt { get; set; }

    public bool IsPaid { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
