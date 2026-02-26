using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Common.DTO_s
{
    // Used when a new user signs up
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName);

    // Used for the login attempt
    public record LoginRequest(
        string Email,
        string Password);

    // The object returned to the frontend/Postman upon successful login
    public record AuthResponse(
        string Token,
        string Email,
        string FirstName);
}
