using System;
using System.Collections.Generic;

namespace EFCoreDBFirstApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? AddressId { get; set; }

    public string? Role { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
