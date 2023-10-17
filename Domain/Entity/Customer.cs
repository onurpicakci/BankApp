using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Customer
{
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string TCKN { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public DateTime Birthdate { get; set; }
    public string CustomerNo { get; set; }
}