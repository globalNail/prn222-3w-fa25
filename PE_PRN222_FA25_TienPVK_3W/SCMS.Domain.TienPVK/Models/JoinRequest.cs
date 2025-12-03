using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class JoinRequest
{
    public int JoinRequestId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public int MemberId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string? Message { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ApprovedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public int? ApprovedByUserId { get; set; }

    public bool IsFeePaid { get; set; }

    public decimal RequiredFeeAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual SystemAccount? ApprovedByUser { get; set; }

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;

    public virtual ICollection<JoinRequestReview> JoinRequestReviews { get; set; } = new List<JoinRequestReview>();

    public virtual Member Member { get; set; } = null!;
}
