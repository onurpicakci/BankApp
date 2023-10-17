using Application.Interfaces;
using DataAccess.Abstract;
using Domain.Entity;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<Customer> AddCustomerAsync(Customer customer)
    {
        return _customerRepository.AddCustomerAsync(customer);
    }

    public Task<Customer> GetCustomerByTCKNAsync(string tckn)
    {
        return _customerRepository.GetCustomerByTCKNAsync(tckn);
    }

    public Task<List<Customer>> GetAllCustomersAsync()
    {
        return _customerRepository.GetAllCustomersAsync();
    }
}