using System.Diagnostics;
using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BankApp.Models;

namespace BankApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountService _accountService;
    private readonly IAccountActivitiesService _accountActivitiesService;

    public HomeController(ILogger<HomeController> logger, IAccountService accountService, IAccountActivitiesService accountActivitiesService)
    {
        _logger = logger;
        _accountService = accountService;
        _accountActivitiesService = accountActivitiesService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var customerNo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (customerNo == null || 
            (await _accountService.GetAccountNoByCustomerNoAsync(customerNo) is not { } account) || 
            (await _accountService.GetAccountsByCustomerNoAsync(account.CustomerNo) is not { } getAccount))
        {
            return RedirectToAction("Index", "CustomerList");
        }

        return View(getAccount.ToList());
    }
    
    [HttpGet]
    public async Task<IActionResult> AccountDetail(string accountNumber)
    {
        if (accountNumber == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var account = await _accountService.GetAccountByAccountNoAsync(accountNumber);
        
        if (account == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        AccountDetailViewModel accountDetailViewModel = new()
        {
            Account = account,
            AccountActivities = await _accountActivitiesService.GetLastThreeActivitiesAsync(accountNumber)
        };
        
        return View(accountDetailViewModel);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}