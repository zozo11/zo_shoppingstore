using System;

namespace ZoOnlineStore.Models;

public class Inventory
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public required string Action { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public required Product Product { get; set; }
}
