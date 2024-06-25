using System;
using System.Collections.Generic;

namespace EFCoreDBFirstApp.Models;

public partial class CartItem
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? ProductTypeId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public virtual User? User { get; set; }
}
