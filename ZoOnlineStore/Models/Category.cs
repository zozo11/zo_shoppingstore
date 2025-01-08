namespace ZoOnlineStore.Models;

public class Category
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public int? ParentID { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category>? SubCategories { get; set; }
    public ICollection<Product>? Products { get; set; }
}
