using System;

namespace ZoOnlineStore.Models;

public class Product
{
    public int ID { get; set; }
    public required string UUID { get; set; }
    public required string Name { get; set; }
    public int CategoryID { get; set; }
    public required string SKU { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public required Category Category { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
    public required ICollection<Inventory> Inventories { get; set; }
}
