using ToDoApp.Shared.Models;

namespace ToDoApp.ApplicationLayer.Services.Contracts;

public interface IAuthService
{
	Task<UserCredentialModel> Register(AuthModel model);

	Task<UserCredentialModel> Login(AuthModel model);
}