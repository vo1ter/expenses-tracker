using backend.Common.Enums;

namespace backend.Domain.Entities;

public class RecurringTransaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string? Note { get; private set; }
    public RecurrenceFrequency Frequency { get; private set; }
    public int Interval { get; private set; } // e.g. every 2 weeks
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime NextRunDate { get; private set; }

    private RecurringTransaction() { }

    public RecurringTransaction(Guid accountId, Guid categoryId, decimal amount, TransactionType type,
        RecurrenceFrequency frequency, int interval, DateTime startDate, DateTime? endDate, string? note = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        if (interval <= 0)
            throw new ArgumentException("Interval must be at least 1.");

        Id = Guid.NewGuid();
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        Type = type;
        Frequency = frequency;
        Interval = interval;
        StartDate = startDate;
        EndDate = endDate;
        NextRunDate = startDate;
        Note = note;
    }

    public Transaction GenerateTransaction()
    {
        var transaction = new Transaction(AccountId, CategoryId, Amount, Type, NextRunDate, Note);
        AdvanceNextRunDate();
        return transaction;
    }

    private void AdvanceNextRunDate()
    {
        NextRunDate = Frequency switch
        {
            RecurrenceFrequency.Daily => NextRunDate.AddDays(Interval),
            RecurrenceFrequency.Weekly => NextRunDate.AddDays(7 * Interval),
            RecurrenceFrequency.Monthly => NextRunDate.AddMonths(Interval),
            RecurrenceFrequency.Yearly => NextRunDate.AddYears(Interval),
            _ => NextRunDate
        };
    }

    public bool IsDue(DateTime asOf) =>
        NextRunDate <= asOf && (EndDate is null || NextRunDate <= EndDate);
}