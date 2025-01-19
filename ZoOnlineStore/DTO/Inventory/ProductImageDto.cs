using System;

namespace ZoOnlineStore.DTO.Inventory;

public class ProductImageDto
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsThumbnailImage { get; set; }

}
