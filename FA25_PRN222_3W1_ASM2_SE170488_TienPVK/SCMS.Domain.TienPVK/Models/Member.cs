using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public int UserId { get; set; }

    public string StudentCode { get; set; } = null!;

    public string? Faculty { get; set; }

    public string? Major { get; set; }

    public int? IntakeYear { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public string? AvatarUrl { get; set; }

    public decimal? Gpa { get; set; }

    public bool IsVerified { get; set; }

    public bool IsGraduated { get; set; }

    public DateTime? JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = new List<ActivityRegistration>();

    public virtual ICollection<AttendanceCheck> AttendanceChecks { get; set; } = new List<AttendanceCheck>();

    public virtual ICollection<DisciplinaryCase> DisciplinaryCases { get; set; } = new List<DisciplinaryCase>();

    public virtual ICollection<FeeInvoice> FeeInvoices { get; set; } = new List<FeeInvoice>();

    public virtual ICollection<JoinRequest> JoinRequests { get; set; } = new List<JoinRequest>();

    public virtual ICollection<MemberDocument> MemberDocuments { get; set; } = new List<MemberDocument>();

    public virtual SystemAccount User { get; set; } = null!;
}
