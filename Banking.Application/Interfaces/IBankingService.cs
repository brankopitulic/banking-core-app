using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;

public interface IBankingService
{
    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto);
    Task<AccountDto?> GetAccountByNumberAsync(string accountNumber);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<bool> DepositAsync(DepositDto dto);
    Task<bool> WithdrawAsync(WithdrawDto dto);
    Task<bool> TransferAsync(TransferDto dto);
}
