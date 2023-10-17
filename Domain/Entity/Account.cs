using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Account
{
    public string AccountNumber { get; set; }

    public string CustomerNo { get; set; }

    public decimal Balance { get; set; }

    [Required]
    public int AccountType { get; set; }
    
    [Required]
    [Range(1, Int32.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
    public string DestinationAccountNumber { get; set; }
}