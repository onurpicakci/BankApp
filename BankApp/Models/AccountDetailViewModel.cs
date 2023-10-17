using Domain.Entity;

namespace BankApp.Models;

public class AccountDetailViewModel
{
    public Account Account { get; set; }
    
    public List<AccountActivities> AccountActivities { get; set; }
}