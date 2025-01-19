using System;

namespace ZoOnlineStore.Models;

public class ProductImage
{   
    public int ID { get; set; }
    public int ProductID { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsThumbnailImage { get; set; }
    public virtual Product Product { get; set; }
}
