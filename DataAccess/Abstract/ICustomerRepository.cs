using Domain.Entity;

namespace DataAccess.Abstract;

public interface ICustomerRepository
{
    Task<Customer> AddCustomerAsync(Customer customer);
    Task<Customer> GetCustomerByTCKNAsync(string tckn);
    
    Task<List<Customer>> GetAllCustomersAsync();
}