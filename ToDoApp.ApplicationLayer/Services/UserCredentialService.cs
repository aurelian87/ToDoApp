using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.ApplicationLayer.Services;

public class UserCredentialService : IUserCredentialService
{
	private readonly IUserCredentialRepository _userCredentialRepository;

	public UserCredentialService(IUserCredentialRepository userCredentialRepository)
	{
		_userCredentialRepository = userCredentialRepository;
	}

	public async Task<UserCredentialModel?> GetById(int id)
	{
		return await _userCredentialRepository.GetById(id);
	}

	public async Task<UserCredentialModel?> GetByUserName(string userName)
	{
		return await _userCredentialRepository.GetByUserName(userName);
	}

	public async Task<UserCredentialModel> Add(UserCredentialModel userCredential)
	{
		return await _userCredentialRepository.Add(userCredential);
	}

	public async Task<UserCredentialModel> Register(UserCredentialModel userCredential)
	{
		return await _userCredentialRepository.Register(userCredential);
	}

	public async Task<UserCredentialModel> Login(UserCredentialModel userCredential)
	{
		return await _userCredentialRepository.Login(userCredential);
	}
}