using ToDoApp.Shared.Models;

namespace ToDoApp.ApplicationLayer.Services.Contracts;

public interface IUserCredentialService
{
	Task<UserCredentialModel?> GetById(int id);

	Task<UserCredentialModel?> GetByUserName(string userName);

	Task<UserCredentialModel> Add(UserCredentialModel userCredential);

	Task<UserCredentialModel> Register(UserCredentialModel userCredential);

	Task<UserCredentialModel> Login(UserCredentialModel userCredential);
}