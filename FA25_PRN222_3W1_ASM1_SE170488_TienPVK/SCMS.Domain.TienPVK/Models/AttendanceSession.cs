using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class AttendanceSession
{
    public int AttendanceSessionId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public int? ActivityId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime SessionDate { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string? Location { get; set; }

    public int? HostUserId { get; set; }

    public bool IsMandatory { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Activity? Activity { get; set; }

    public virtual ICollection<AttendanceCheck> AttendanceChecks { get; set; } = new List<AttendanceCheck>();

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;

    public virtual SystemAccount? HostUser { get; set; }
}
