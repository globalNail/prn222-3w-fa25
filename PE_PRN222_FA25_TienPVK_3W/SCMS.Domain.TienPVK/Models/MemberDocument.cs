using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class MemberDocument
{
    public int MemberDocumentId { get; set; }

    public int MemberId { get; set; }

    public string DocType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public bool IsVerified { get; set; }

    public bool IsExpired { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public string? Note { get; set; }

    public virtual Member Member { get; set; } = null!;
}
