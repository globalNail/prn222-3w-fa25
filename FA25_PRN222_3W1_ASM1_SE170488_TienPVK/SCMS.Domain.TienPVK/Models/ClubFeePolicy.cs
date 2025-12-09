using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class ClubFeePolicy
{
    public int FeePolicyId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public string FeeType { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Period { get; set; } = null!;

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public bool IsMandatory { get; set; }

    public bool IsRefundable { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;

    public virtual ICollection<FeeInvoiceLine> FeeInvoiceLines { get; set; } = new List<FeeInvoiceLine>();
}
