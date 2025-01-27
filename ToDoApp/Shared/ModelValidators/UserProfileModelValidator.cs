using FluentValidation;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.ResourceFiles;

namespace ToDoApp.Shared.ModelValidators;

public class UserProfileModelValidator : AbstractValidator<UserProfileModel>
{
	public UserProfileModelValidator()
	{
		RuleFor(u => u.FirstName).NotEmpty().WithMessage("FirstName is required");
		RuleFor(u => u.LastName).NotEmpty().WithMessage("LastName is required");
		RuleFor(u => u.Email).EmailAddress().WithMessage("Email is not valid");
	}
}