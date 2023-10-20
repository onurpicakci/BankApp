using Domain.Entity;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Account> AddAccountAsync(Account account);
    
    Task<Account> DepositAsync(string accountNumber, decimal amount);
    
    Task<Account> GetAccountNoByCustomerNoAsync(string customerNo);
    
    Task<Account> GetAccountByAccountNoAsync(string accountNo);
    
    Task<Account> TransferMoneyAsync(string accountNumber, string destinationAccountNumber, decimal amount);
    
    Task<Account> WithdrawAsync(string accountNumber, decimal amount);
    
    Task<List<Account>> GetAccountsByCustomerNoAsync(string customerNo);
    
    Task<List<Account>> GetAccountsByDifferentCustomerNoAsync(string customerNo);
}
