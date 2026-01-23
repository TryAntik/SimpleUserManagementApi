using FluentValidation;
using SimpleUserManagementApi.Auth.DTOs;

namespace SimpleUserManagementApi.UserManager.Validators;

public class LoginRequestDTOValidator : AbstractValidator<LoginRequestDTO>
{
    public LoginRequestDTOValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("email is required")
            .EmailAddress().WithMessage("invalid email format");

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("password is required")
            .MinimumLength(6).WithMessage("password must have at least 6 characters")
            .MaximumLength(36).WithMessage("password must have less than 36 characters");
    }
}