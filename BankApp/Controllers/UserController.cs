using System.Security.Claims;
using Application.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        await _userService.AddUserAsync(user);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var userFromDb = await _userService.GetUserByUsernameAsync(user.Username);

        if (userFromDb == null)
        {
            ModelState.AddModelError("Username", "Kullanıcı adı bulunamadı.");
            return RedirectToAction("AddUser", "User");
        }

        if (userFromDb.Username != user.Username || userFromDb.Password != user.Password)
        {
            ModelState.AddModelError("Username", "Kullanıcı adı veya şifre hatalı.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userFromDb.Username),
            new Claim(ClaimTypes.Role, userFromDb.Role),
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "YourUserAuthenticationScheme"); 

        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
            AllowRefresh = true
        };
        await HttpContext.SignInAsync(
            "YourUserAuthenticationScheme",
            new ClaimsPrincipal(claimsIdentity),
            authenticationProperties);

        return RedirectToAction("Index", "CustomerList");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("YourUserAuthenticationScheme");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

}