using System;
using System.Collections.Generic;

namespace SCMS.Domain.TienPVK.Models;

public partial class FeeInvoice
{
    public int FeeInvoiceId { get; set; }

    public int ClubIdtienPvk { get; set; }

    public int PayerMemberId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? ProviderTxnId { get; set; }

    public bool IsPaid { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ClubsTienPvk ClubIdtienPvkNavigation { get; set; } = null!;

    public virtual ICollection<FeeInvoiceLine> FeeInvoiceLines { get; set; } = new List<FeeInvoiceLine>();

    public virtual Member PayerMember { get; set; } = null!;
}
