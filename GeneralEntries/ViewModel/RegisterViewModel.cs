using System.ComponentModel.DataAnnotations;

namespace GeneralEntries.ViewModel;

public class RegisterViewModel
{
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
    public required string Password { get; set; }

    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password & Confirmed Password not matched.")]
    public required string ConfirmPassword { get; set; }

    public string Role { get; set; } = string.Empty;
}
