using System.Security.Claims;
using Application.Interfaces;
using BankApp.Controllers;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankApp_Test.ControllerTest;

public class AccountControllerTest
{
    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        var mockRepo = new Mock<IAccountService>();
        var controller = new AccountController(mockRepo.Object);

        var result = await controller.Index("1234567890");

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_ValidAccount_ReturnsRedirectToActionResult()
    {
        var mockRepo = new Mock<IAccountService>();
        mockRepo.Setup(repo => repo.AddAccountAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) =>
            {
                a.AccountNumber = "00000000000000065";
                return a;
            });

        var controller = new AccountController(mockRepo.Object);
        var account = new Account
        {
            CustomerNo = "000000000128",
            Balance = 1000,
            AccountType = 1
        };

        var result = await controller.Index(account);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task Index_InvalidAccount_ReturnsViewResult()
    {
        var mockRepo = new Mock<IAccountService>();
        var controller = new AccountController(mockRepo.Object);
        var account = new Account
        {
            CustomerNo = "000000000",
            Balance = 5,
            AccountType = 5,
        };

        controller.ModelState.AddModelError("CustomerNo", "CustomerNo is required");
    }

    [Fact]
    public async Task Deposit_ReturnsViewResult()
    {
        var mockRepo = new Mock<IAccountService>();
        var controller = new AccountController(mockRepo.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        var result = await controller.Deposit();

        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Deposit_ValidAccount_ReturnsRedirectToActionResult()
    {
        var mockRepo = new Mock<IAccountService>();
        mockRepo.Setup(repo => repo.DepositAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .ReturnsAsync((string accountNumber, decimal amount) =>
            {
                return new Account
                {
                    AccountNumber = accountNumber,
                    Balance = amount
                };
            });

        var controller = new AccountController(mockRepo.Object);
        var account = new Account
        {
            AccountNumber = "00000000000000010",
            Balance = 1000,
            Amount = 1000
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        var result = await controller.Deposit(account.AccountNumber, account.Amount);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Home", redirectToActionResult.ControllerName);
    }
    
    [Fact]
    public async Task Deposit_InvalidAccount_ReturnsViewResult()
    {
        var mockRepo = new Mock<IAccountService>();
        var controller = new AccountController(mockRepo.Object);
        var account = new Account
        {
            AccountNumber = "00000000000000011",
            Balance = 1000,
            Amount = 0
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        controller.ModelState.AddModelError("Amount", "Amount must be greater than 0");
        
        var result = await controller.Deposit(account.AccountNumber, account.Amount);
        
        Assert.IsType<ViewResult>(result);
        
    }
}