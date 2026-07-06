using backend.Common.Enums;

namespace backend.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public decimal Balance { get; private set; }
    public Currency Currency { get; private set; }
    public string Name { get; private set; }
    
    private Account() { }
    
    public Account(Guid userId, string name, Currency currency, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name is required.");

        Id = Guid.NewGuid();
        Name = name;
        Currency = currency;
        Balance = initialBalance;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Account name is required.");
        Name = newName;
    }

    public void ApplyTransaction(decimal amount, TransactionType type)
    {
        Balance += type == TransactionType.Income ? amount : -amount;
    }
}