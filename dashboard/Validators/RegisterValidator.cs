using FluentValidation;
using Dashboard.Application.ViewModels;

namespace dashboard.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("username required")
                .MinimumLength(3).WithMessage("at least 3 char");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email required")
                .EmailAddress().WithMessage("erorr");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password required.")
                .MinimumLength(6).WithMessage("at least 6");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("confirm password required")
                .Equal(x => x.Password).WithMessage("not matched");
        }
    }
}