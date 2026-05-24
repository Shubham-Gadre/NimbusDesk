using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Identity
{
    /// <summary>
    /// Defines the contract for generating JWT tokens for authenticated users.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user with their assigned roles.
        /// </summary>
        /// <param name="user">The application user for whom to generate the token.</param>
        /// <param name="roles">The collection of roles assigned to the user to include in the token claims.</param>
        /// <returns>A JWT token string that can be used for API authentication.</returns>
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
    }
}
