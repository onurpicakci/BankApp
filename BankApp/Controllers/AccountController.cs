using System.Security.Claims;
using Application.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankApp.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string customerNo)
    {
        if (!string.IsNullOrEmpty(customerNo) && customerNo.Length < 12)
        {
            customerNo = customerNo.PadLeft(12, '0');
        }

        ViewBag.CustomerNo = customerNo;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Account account)
    {
        try
        {
            await _accountService.AddAccountAsync(account);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Deposit()
    {
        await GetAccountNumber();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(string accountNumber, decimal amount)
    {
        if (amount <= 0)
        {
            ModelState.AddModelError("Amount", "Amount must be greater than 0");
            await GetAccountNumber();
            return View();
        }

        try
        {
            await _accountService.DepositAsync(accountNumber, amount);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> TransferMoney()
    {
        await GetAccountNumber();
        await GetDestinationAccountNumber();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TransferMoney(string accountNumber, string destinationAccountNumber,
        decimal amount)
    {
        if (amount <= 0)
        {
            ModelState.AddModelError("Amount", "Amount must be greater than 0");
            await GetAccountNumber();
            await GetDestinationAccountNumber();
            return View();
        }

        try
        {
            await _accountService.TransferMoneyAsync(accountNumber, destinationAccountNumber, amount);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Withdraw()
    {
        await GetAccountNumber();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw(string accountNumber, decimal amount)
    {
        if (amount <= 0)
        {
            ModelState.AddModelError("Amount", "Amount must be greater than 0");
            await GetAccountNumber();
            return View();
        }

        try
        {
            await _accountService.WithdrawAsync(accountNumber, amount);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task GetAccountNumber()
    {
        List<string> accountNumbers;
        var customerNo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (customerNo == null)
        {
            return;
        }

        var accounts = await _accountService.GetAccountsByCustomerNoAsync(customerNo);
        accountNumbers = accounts.Select(x => x.AccountNumber).ToList();
        ViewBag.AccountNumbers = new SelectList(accountNumbers);
    }
    
    public async Task GetDestinationAccountNumber()
    {
        List<string> destinationAccountNumbers;
        var customerNo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (customerNo == null)
        {
            return;
        }

        var accounts = await _accountService.GetAccountsByDifferentCustomerNoAsync(customerNo);
        destinationAccountNumbers = accounts.Select(x => x.AccountNumber).ToList();
        ViewBag.DifferentAccountNumbers = new SelectList(destinationAccountNumbers);
    }
}