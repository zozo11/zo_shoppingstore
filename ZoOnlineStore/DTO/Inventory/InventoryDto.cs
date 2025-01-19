using System;

namespace ZoOnlineStore.DTO.Inventory;

public class InventoryDto
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public string Action { get; set; }

}
