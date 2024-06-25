using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EFCoreDBFirstApp.Models;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ProductTypeId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? OriginalPrice { get; set; }

    public bool? Visible { get; set; }

    public bool? Deleted { get; set; }

    //public bool? IsNew { get; set; }
    [JsonIgnore]
    public virtual Product? Product { get; set; }

    public virtual Category? ProductType { get; set; }
}
