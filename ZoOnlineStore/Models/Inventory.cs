using System;

namespace ZoOnlineStore.Models;

public class Inventory
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public required string Action { get; set; } //in store / pending / outstore 
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual Product Product { get; set; }
}
