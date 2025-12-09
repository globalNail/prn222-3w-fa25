using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class DisciplinaryCase
{
    public int CaseId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public int MemberId { get; set; }

    public DateTime OpenedAt { get; set; }

    public int? ReportedByUserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Severity { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;

    public virtual ICollection<DisciplinaryAction> DisciplinaryActions { get; set; } = new List<DisciplinaryAction>();

    public virtual Member Member { get; set; } = null!;

    public virtual SystemAccount? ReportedByUser { get; set; }
}
