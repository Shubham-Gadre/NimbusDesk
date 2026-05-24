using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Common.DTO_s
{
    /// <summary>
    /// Represents a user registration request containing credentials and profile information.
    /// </summary>
    /// <param name="Email">The email address of the user (must be unique).</param>
    /// <param name="Password">The password for the user account (must meet strength requirements).</param>
    /// <param name="FirstName">The first name of the user.</param>
    /// <param name="LastName">The last name of the user.</param>
    // Used when a new user signs up
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName);

    /// <summary>
    /// Represents a user login request with email and password credentials.
    /// </summary>
    /// <param name="Email">The email address of the user.</param>
    /// <param name="Password">The password of the user.</param>
    // Used for the login attempt
    public record LoginRequest(
        string Email,
        string Password);

    /// <summary>
    /// Represents the response returned upon successful user authentication.
    /// Contains the JWT token and basic user information.
    /// </summary>
    /// <param name="Token">The JWT token to be used for subsequent authenticated requests.</param>
    /// <param name="Email">The email address of the authenticated user.</param>
    /// <param name="FirstName">The first name of the authenticated user.</param>
    // The object returned to the frontend/Postman upon successful login
    public record AuthResponse(
        string Token,
        string Email,
        string FirstName);
}
