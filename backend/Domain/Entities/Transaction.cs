using backend.Common.Enums;

namespace backend.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Date { get; private set; }
    public string? Note { get; private set; }
    public Guid? RecurringTransactionId { get; private set; }
    
    private Transaction() { }

    public Transaction(Guid accountId, Guid categoryId, decimal amount, TransactionType type, DateTime date, string? note = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Id = Guid.NewGuid();
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        Type = type;
        Date = date;
        Note = note;
    }

    public void Edit(decimal amount, Guid categoryId, DateTime date, string? note)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Amount = amount;
        CategoryId = categoryId;
        Date = date;
        Note = note;
    }
}