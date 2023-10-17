using Application.Interfaces;
using DataAccess.Abstract;
using Domain.Entity;


namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task<Account> AddAccountAsync(Account account)
    {
        return _accountRepository.AddAccountAsync(account);
    }

    public Task<Account> DepositAsync(string accountNumber, decimal amount)
    {
        return _accountRepository.DepositAsync(accountNumber, amount);
    }

    public Task<Account> GetAccountNoByCustomerNoAsync(string customerNo)
    {
        return _accountRepository.GetAccountNoByCustomerNoAsync(customerNo);
    }

    public Task<Account> GetAccountByAccountNoAsync(string accountNo)
    {
        return _accountRepository.GetAccountByAccountNoAsync(accountNo);
    }

    public Task<Account> TransferMoneyAsync(string accountNumber, string destinationAccountNumber, decimal amount)
    {
        return _accountRepository.TransferMoneyAsync(accountNumber, destinationAccountNumber, amount);
    }

    public Task<Account> WithdrawAsync(string accountNumber, decimal amount)
    {
        return _accountRepository.WithdrawAsync(accountNumber, amount);
    }

    public Task<List<Account>> GetAccountsByCustomerNoAsync(string customerNo)
    {
        return _accountRepository.GetAccountsByCustomerNoAsync(customerNo);
    }
}