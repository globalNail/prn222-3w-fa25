using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class JoinRequestReview
{
    public int ReviewId { get; set; }

    public int JoinRequestId { get; set; }

    public int ReviewerUserId { get; set; }

    public int StepNo { get; set; }

    public string Decision { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public bool IsFinal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual JoinRequest JoinRequest { get; set; } = null!;

    public virtual SystemAccount ReviewerUser { get; set; } = null!;
}
