using Application.Interfaces;
using BankApp.Controllers;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankApp_Test.ControllerTest;

public class CustomerControllerTest
{
    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        var mockRepo = new Mock<ICustomerService>();
        var controller = new CustomerController(mockRepo.Object);

        var result = await controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_ValidTCKN_ReturnsRedirectToActionResult()
    {
        var mockRepo = new Mock<ICustomerService>();
        mockRepo.Setup(repo => repo.AddCustomerAsync(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) =>
            {
                c.CustomerNo = "1234567890";
                return c;
            });

        var controller = new CustomerController(mockRepo.Object);
        var customer = new Customer
        {
            TCKN = "62548173646",
            Name = "Onur",
            Surname = "Pıçakcı",
            Address = "Test",
            Birthdate = new DateTime(2001, 4, 29)
        };

        var result = await controller.Index(customer);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task Index_InvalidTCKN_ReturnsViewResult()
    {
        var mockRepo = new Mock<ICustomerService>();
        var controller = new CustomerController(mockRepo.Object);
        var customer = new Customer
        {
            TCKN = "62548173645",
            Name = "Onur",
            Surname = "Pıçakcı",
            Address = "Test",
            Birthdate = new DateTime(2001, 4, 29)
        };

        var result = await controller.Index(customer);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("TCKN ve bilgileriniz uyuşmuyor.", viewResult.ViewData["Message"]);
    }
    
}