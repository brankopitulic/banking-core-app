namespace Banking.Domain.Entities;

public class Account
{
    private static int _accountCounter = 1000;

    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; }
    public string AccountHolderName { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private Account() { }

    public Account(string accountHolderName, decimal initialDeposit)
    {
        if (string.IsNullOrWhiteSpace(accountHolderName))
            throw new ArgumentException("Account holder name is required.");
        
        if (initialDeposit < 0)
            throw new ArgumentException("Initial deposit cannot be negative.");

        Id = Guid.NewGuid();
        AccountNumber = GenerateAccountNumber();
        AccountHolderName = accountHolderName;
        Balance = initialDeposit;
        CreatedDate = DateTime.UtcNow;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
    }

    private static string GenerateAccountNumber()
    {
        return $"ACC{Interlocked.Increment(ref _accountCounter):D6}";
    }
}
