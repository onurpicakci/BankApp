using System.Security.Claims;
using Application.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.ViewComponents;

public class Sidebar : ViewComponent
{
    private readonly ICustomerService _customerService;

    public Sidebar(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (result.Succeeded)
        {
            var tcknClaim = result.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (tcknClaim != null)
            {
                var customer = await _customerService.GetCustomerByTCKNAsync(tcknClaim.Value);

                return View("Default", customer);
            }
        }

        return View("Default", new Customer());
    }
}