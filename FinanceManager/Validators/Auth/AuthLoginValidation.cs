using FinanceManager.Dtos.Auth;
using FluentValidation;

namespace FinanceManager.Validators.Auth
{
    public class AuthLoginValidation : AbstractValidator<AuthLoginDto>
    {
        public AuthLoginValidation() 
        { 
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
