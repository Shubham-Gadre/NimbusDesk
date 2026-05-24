using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NimbusDesk.Infrastructure.Identity
{
    /// <summary>
    /// Provides JWT token generation for user authentication.
    /// Creates tokens with user claims, roles, and configured expiration settings.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="config">The application configuration containing JWT settings.</param>
        public TokenService(IConfiguration config) => _config = config;

        /// <summary>
        /// Generates a JWT token for the specified user with their roles.
        /// The token includes user identity claims and assigned role claims.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="roles">The roles to include in the token claims.</param>
        /// <returns>A JWT token string valid for 15 minutes.</returns>
        public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // PRODUCTION FIX: Use UtcNow instead of Now, reduce expiration to 15 minutes
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),  // Changed from 3 hours to 15 minutes
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

