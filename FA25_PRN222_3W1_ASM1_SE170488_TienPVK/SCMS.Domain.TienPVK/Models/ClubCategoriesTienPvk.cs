using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class ClubCategoriesTienPvk
{
    public int CategoryIdtienPvk { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<ClubsTienPvk> ClubsTienPvks { get; set; } = new List<ClubsTienPvk>();
}
