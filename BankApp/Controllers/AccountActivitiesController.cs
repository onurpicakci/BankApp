using Application.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace BankApp.Controllers;

public class AccountActivitiesController : Controller
{
    private readonly IAccountActivitiesService _accountActivitiesService;

    public AccountActivitiesController(IAccountActivitiesService accountActivitiesService)
    {
        _accountActivitiesService = accountActivitiesService;
    }

    [Route("AccountActivities/{accountNumber}/{page?}")]
    [HttpGet]
    public async Task<IActionResult> Index(string accountNumber, int page = 1, int pageSize = 10)
    {
        var accountActivities = await _accountActivitiesService.GetAccountActivitiesAsync(accountNumber);
        var pagedList = new PagedList<AccountActivities>(accountActivities.AsQueryable(), page, pageSize);
        return View(pagedList);
    }
}