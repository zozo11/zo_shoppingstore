using System;

namespace ZoOnlineStore.DTO.Category;

public class CategoriesDto
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public int? ParentID { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

}
