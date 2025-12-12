using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class ClubsTienPvk
{
    public int ClubIdtienPvk { get; set; }

    public string ClubCode { get; set; } = null!;

    public string ClubName { get; set; } = null!;

    public int CategoryIdtienPvk { get; set; }

    public string? Description { get; set; }

    public DateTime FoundedDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? ManagerUserId { get; set; }

    public int MemberLimit { get; set; }

    public bool IsOpenToJoin { get; set; }

    public bool RequiresApproval { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();

    public virtual ClubCategoriesTienPvk Category { get; set; } = null!;

    public virtual ICollection<ClubFeePolicy> ClubFeePolicies { get; set; } = new List<ClubFeePolicy>();

    public virtual ICollection<DisciplinaryCase> DisciplinaryCases { get; set; } = new List<DisciplinaryCase>();

    public virtual ICollection<FeeInvoice> FeeInvoices { get; set; } = new List<FeeInvoice>();

    public virtual ICollection<JoinRequest> JoinRequests { get; set; } = new List<JoinRequest>();

    public virtual SystemAccount? ManagerUser { get; set; }
}
