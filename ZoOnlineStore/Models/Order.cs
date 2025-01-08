using System;

namespace ZoOnlineStore.Models;

public class Order
{
    public int ID { get; set; }
    public required string UUID { get; set; }
    public required string OrderNumber { get; set; }
    public required int UserID { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
}
