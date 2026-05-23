using Dashboard.Application.ViewModels;
using FluentValidation;


namespace dashboard.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("username required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password required.");
        }
    }
}