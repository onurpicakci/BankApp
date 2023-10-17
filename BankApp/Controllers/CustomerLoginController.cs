using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

[Authorize(AuthenticationSchemes = "YourUserAuthenticationScheme")]
public class CustomerLoginController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IAccountService _accountService;
    
    public CustomerLoginController(ICustomerService customerService, IAccountService accountService)
    {
        _customerService = customerService;
        _accountService = accountService;
    }
    
    [HttpGet]
    public IActionResult Index(string tckn)
    {
        ViewBag.TCKN = tckn;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string tckn, string name)
    {
        var customer = await _customerService.GetCustomerByTCKNAsync(tckn);
        var account = await _accountService.GetAccountNoByCustomerNoAsync(customer.CustomerNo);

        if (customer == null)
        {
            ViewBag.Message = "Müşteri bulunamadı.";
            return View();
        }

        if (customer.Name != name)
        {
            ViewBag.Message = "Hatalı isim girdiniz.";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, customer.TCKN),
            new Claim(ClaimTypes.Role, "Customer"),
            new Claim(ClaimTypes.NameIdentifier, customer.CustomerNo),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
            AllowRefresh = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authenticationProperties);

        if (account == null)
        {
            return RedirectToAction("Index", "Account", new { customerNo = customer.CustomerNo });
        }

        return RedirectToAction("Index", "Home", new { customerNo = customer.CustomerNo });
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}