using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class FeeInvoiceLine
{
    public int FeeInvoiceLineId { get; set; }

    public int FeeInvoiceId { get; set; }

    public int? FeePolicyId { get; set; }

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public bool IsDiscountLine { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual FeeInvoice FeeInvoice { get; set; } = null!;

    public virtual ClubFeePolicy? FeePolicy { get; set; }
}
