using Microsoft.AspNetCore.Identity;

namespace Products.API.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the initials of the user.
    /// </summary>
    public string? Initials { get; set; }
}
