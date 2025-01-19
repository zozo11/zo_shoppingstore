using System;

namespace ZoOnlineStore.Models;

public class Product
{
    public int ID { get; set; }
    public string UUID { get; set; }
    public required string Name { get; set; }
    public int CategoryID { get; set; }
    public required string SKU { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public int InventoryID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual  Category Category { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
    public virtual Inventory Inventory { get; set; }

    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
