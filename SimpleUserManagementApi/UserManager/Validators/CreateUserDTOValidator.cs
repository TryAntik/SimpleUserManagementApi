using FluentValidation;
using SimpleUserManagementApi.UserManager.DTOs;

namespace SimpleUserManagementApi.UserManager.Validators;

public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("email is required")
            .EmailAddress().WithMessage("invalid email format");
        
        RuleFor(a => a.PasswordHJ)
    }
}