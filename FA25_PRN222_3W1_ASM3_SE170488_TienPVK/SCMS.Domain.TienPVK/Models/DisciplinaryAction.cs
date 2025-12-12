using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class DisciplinaryAction
{
    public int ActionId { get; set; }

    public int CaseId { get; set; }

    public string ActionType { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    public bool IsNotified { get; set; }

    public DateTime? NotifiedAt { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual DisciplinaryCase Case { get; set; } = null!;
}
