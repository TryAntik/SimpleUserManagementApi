using FluentValidation;
using SimpleUserManagementApi.UserManager.DTOs;

namespace SimpleUserManagementApi.UserManager.Validators;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("email is required")
            .EmailAddress().WithMessage("invalid email format");

        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("name is required")
            .MinimumLength(3).WithMessage("name must have at least 3 characters")
            .MaximumLength(13).WithMessage("name must have less than 13 characters");
    }
}