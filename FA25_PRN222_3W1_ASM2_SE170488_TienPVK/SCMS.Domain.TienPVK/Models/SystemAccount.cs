using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class SystemAccount
{
    public int UserAccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public int RoleId { get; set; }

    public string? RequestCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ApplicationCode { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();

    public virtual ICollection<ClubsTienPvk> ClubsTienPvks { get; set; } = new List<ClubsTienPvk>();

    public virtual ICollection<DisciplinaryCase> DisciplinaryCases { get; set; } = new List<DisciplinaryCase>();

    public virtual ICollection<JoinRequestReview> JoinRequestReviews { get; set; } = new List<JoinRequestReview>();

    public virtual ICollection<JoinRequest> JoinRequests { get; set; } = new List<JoinRequest>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
