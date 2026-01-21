using FluentValidation;
using SimpleUserManagementApi.Auth.DTOs;

namespace SimpleUserManagementApi.UserManager.Validators;

public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestDTOValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("email is required")
            .EmailAddress().WithMessage("invalid email format");

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("password is required")
            .MinimumLength(6).WithMessage("password must have at least 6 characters")
            .MaximumLength(36).WithMessage("password must have less than 36 characters");

        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("name is required")
            .MinimumLength(3).WithMessage("name must have at least 3 characters")
            .MaximumLength(13).WithMessage("name must have less than 13 characters");
    }
}