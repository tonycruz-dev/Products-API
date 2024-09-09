using System.Security.Claims;

namespace Products.API.IntegrationTest.Helpers;

/// <summary>
/// Helper class for authentication related operations.
/// </summary>
public class AuthHelper
{
    /// <summary>
    /// Retrieves the bearer token for the specified user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>A dictionary containing the bearer token.</returns>
    public static Dictionary<string, object> GetBearerForUser(string username)
    {
        return new Dictionary<string, object> { { ClaimTypes.Name, username } };
    }
}
