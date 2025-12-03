using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class Activity
{
    public int ActivityId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string? Location { get; set; }

    public int MaxParticipants { get; set; }

    public decimal FeePerPerson { get; set; }

    public bool IsPublic { get; set; }

    public bool IsOnline { get; set; }

    public string Status { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public string? CancelReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = new List<ActivityRegistration>();

    public virtual ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;
}
