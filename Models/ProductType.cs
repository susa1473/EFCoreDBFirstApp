using System;
using System.Collections.Generic;

namespace EFCoreDBFirstApp.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsNewBit { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
