using System;
using System.Collections.Generic;

namespace EFCoreDBFirstApp.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public bool? Visible { get; set; }

    public bool? Deleted { get; set; }

    public bool? IsNew { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
