using backend.Common.Enums;

namespace backend.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public string Name { get; set; }
    public TransactionType Type { get; private set; } // a category is either for expenses or income
    
    private Category() { }
    
    public Category(Guid userId, string name, TransactionType type, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }
}