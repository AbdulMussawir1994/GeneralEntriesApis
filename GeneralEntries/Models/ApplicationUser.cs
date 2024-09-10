using Microsoft.AspNetCore.Identity;

namespace GeneralEntries.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateCreated { get; private set; } = DateTime.UtcNow;
    public DateTime DateModified { get; private set; } = DateTime.UtcNow;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();

    public void UpdateModifiedDate() => DateModified = DateTime.UtcNow;
}