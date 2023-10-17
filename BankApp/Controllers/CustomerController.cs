using Application.Interfaces;
using BankApp.KPSPublic;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(Customer customer)
    {
        try
        {
            if (!ValidateTckn(customer.TCKN, customer.Name, customer.Surname, customer.Birthdate.Year))
            {
                ViewBag.Message = "TCKN ve bilgileriniz uyuşmuyor.";
                return View();
            }
            var customerNumber = await _customerService.AddCustomerAsync(customer);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private bool ValidateTckn(string tckn, string name, string surname, int birthYear)
    {
        try
        {
            var service = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
            var result = service.TCKimlikNoDogrulaAsync(Convert.ToInt64(tckn), name, surname, birthYear).Result;
            return result.Body.TCKimlikNoDogrulaResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}