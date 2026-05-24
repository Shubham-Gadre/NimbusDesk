using FluentValidation;
using NimbusDesk.Application.Common.DTO_s;

namespace NimbusDesk.Application.Identity
{
    /// <summary>
    /// Validator for the RegisterRequest.
    /// Validates user registration data including email format, password strength, and name requirements.
    /// </summary>
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterValidator"/> class.
        /// Configures validation rules for user registration.
        /// </summary>
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain digit")
                .Matches(@"[!@#$%^&*]").WithMessage("Password must contain special character (!@#$%^&*)");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
        }
    }

    /// <summary>
    /// Validator for the LoginRequest.
    /// Validates user login credentials including email format and password presence.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginValidator"/> class.
        /// Configures validation rules for user login.
        /// </summary>
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
