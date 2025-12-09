using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class AttendanceCheck
{
    public int AttendanceCheckId { get; set; }

    public int AttendanceSessionId { get; set; }

    public int MemberId { get; set; }

    public DateTime? CheckedInAt { get; set; }

    public DateTime? CheckedOutAt { get; set; }

    public bool IsLate { get; set; }

    public bool IsExcused { get; set; }

    public bool IsPresent { get; set; }

    public bool IsAbsent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual AttendanceSession AttendanceSession { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
