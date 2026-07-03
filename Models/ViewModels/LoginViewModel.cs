using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Password { get; set; } = string.Empty;
}
