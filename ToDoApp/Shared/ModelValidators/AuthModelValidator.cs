using FluentValidation;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.ResourceFiles;

namespace ToDoApp.Shared.ModelValidators;

public class AuthModelValidator : AbstractValidator<AuthModel>
{
	public AuthModelValidator()
	{
		RuleFor(a => a.UserName).NotEmpty().WithMessage(Resource.er_AuthModel_UserNameRequired);
		RuleFor(a => a.Password).NotEmpty().WithMessage(Resource.er_AuthModel_PasswordRequired);
	}
}