using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;
[Authorize(AuthenticationSchemes = "YourUserAuthenticationScheme")]
public class CustomerListController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerListController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return View(customers.ToList());
    }

}