using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Validations;

namespace Entity.Models;

public partial class LionProfile
{
    public int LionProfileId { get; set; }

    [Required(ErrorMessage = "Lion Type is required.")]
    public int LionTypeId { get; set; }

    [Required(ErrorMessage = "Lion Name is required.")]
    [LionNameValidation]
    public string LionName { get; set; } = null!;

    [Required(ErrorMessage = "Weight is required.")]
    [Range(30.01, double.MaxValue, ErrorMessage = "Weight must be greater than 30.")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Characteristics is required.")]
    public string Characteristics { get; set; } = null!;

    [Required(ErrorMessage = "Warning is required.")]
    public string Warning { get; set; } = null!;

    [Required(ErrorMessage = "Modified Date is required.")]
    public DateTime ModifiedDate { get; set; }

    public virtual LionType LionType { get; set; } = null!;
}
