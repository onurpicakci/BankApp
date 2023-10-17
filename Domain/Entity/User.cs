using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class User
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string Username { get; set; }

    [Required] [MaxLength(100)] public string Email { get; set; }

    [Required] [MaxLength(50)] public string Password { get; set; }

    [Required] [MaxLength(50)] public string ConfirmPassword { get; set; }

    [Required] [MaxLength(50)] public string Role { get; set; }
}