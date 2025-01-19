using System;

namespace ZoOnlineStore.DTO.Inventory;

public class ProductDto
{
    public int ID { get; set; }
    public int OrderID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductID { get; set; }
    public string SKU { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } 
    public int CategoryId { get; set; }
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string? currency { get; set; }
    public int? discount { get; set; }
    public string? Description { get; set; }
    public string Action { get; set; }
    public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
}