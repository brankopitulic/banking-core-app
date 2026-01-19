using Banking.Domain.Entities;
using Banking.Domain.Interfaces;

namespace Banking.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            return _accounts.FirstOrDefault(a => a.Id == id);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        await _lock.WaitAsync();
        try
        {
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return _accounts.ToList();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddAsync(Account account)
    {
        await _lock.WaitAsync();
        try
        {
            _accounts.Add(account);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(Account account)
    {
        await _lock.WaitAsync();
        try
        {
            var existing = _accounts.FirstOrDefault(a => a.Id == account.Id);
            if (existing != null)
            {
                _accounts.Remove(existing);
                _accounts.Add(account);
            }
        }
        finally
        {
            _lock.Release();
        }
    }
}
