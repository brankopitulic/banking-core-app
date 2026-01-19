namespace Banking.Application.DTOs;

public record CreateAccountDto(string AccountHolderName, decimal InitialDeposit);

public record AccountDto(Guid Id, string AccountNumber, string AccountHolderName, decimal Balance, DateTime CreatedDate);

public record DepositDto(string AccountNumber, decimal Amount);

public record WithdrawDto(string AccountNumber, decimal Amount);

public record TransferDto(string FromAccountNumber, string ToAccountNumber, decimal Amount);
