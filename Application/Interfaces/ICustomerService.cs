using Domain.Entity;

namespace Application.Interfaces;

public interface ICustomerService
{
    Task<Customer> AddCustomerAsync(Customer customer);
    Task<Customer> GetCustomerByTCKNAsync(string tckn);
    Task<List<Customer>> GetAllCustomersAsync();
}