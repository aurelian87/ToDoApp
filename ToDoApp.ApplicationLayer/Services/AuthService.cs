using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.ApplicationLayer.Services;

public class AuthService : IAuthService
{
	private readonly IAuthRepository _authRepository;

	public AuthService(IAuthRepository authRepository)
	{
		_authRepository = authRepository;
	}

	public async Task<UserCredentialModel> Register(AuthModel model)
	{
		return await _authRepository.Register(model);
	}

	public async Task<UserCredentialModel> Login(AuthModel model)
	{
		return await _authRepository.Login(model);
	}
}