using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;

namespace Banking.Application.Services;

public class BankingService : IBankingService
{
    private readonly IAccountRepository _accountRepository;

    public BankingService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = new Account(dto.AccountHolderName, dto.InitialDeposit);
        await _accountRepository.AddAsync(account);
        return MapToDto(account);
    }

    public async Task<AccountDto?> GetAccountByNumberAsync(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        return account != null ? MapToDto(account) : null;
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return accounts.Select(MapToDto);
    }

    public async Task<bool> DepositAsync(DepositDto dto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(dto.AccountNumber);
        if (account == null)
            return false;

        account.Deposit(dto.Amount);
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    public async Task<bool> WithdrawAsync(WithdrawDto dto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(dto.AccountNumber);
        if (account == null)
            return false;

        account.Withdraw(dto.Amount);
        await _accountRepository.UpdateAsync(account);
        return true;
    }

    public async Task<bool> TransferAsync(TransferDto dto)
    {
        var fromAccount = await _accountRepository.GetByAccountNumberAsync(dto.FromAccountNumber);
        var toAccount = await _accountRepository.GetByAccountNumberAsync(dto.ToAccountNumber);

        if (fromAccount == null || toAccount == null)
            return false;

        fromAccount.Withdraw(dto.Amount);
        toAccount.Deposit(dto.Amount);

        await _accountRepository.UpdateAsync(fromAccount);
        await _accountRepository.UpdateAsync(toAccount);
        return true;
    }

    private static AccountDto MapToDto(Account account)
    {
        return new AccountDto(
            account.Id,
            account.AccountNumber,
            account.AccountHolderName,
            account.Balance,
            account.CreatedDate
        );
    }
}
