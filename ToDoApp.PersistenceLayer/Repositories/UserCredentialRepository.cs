using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.PersistenceLayer.Repositories;

public class UserCredentialRepository : BaseRepository<UserCredentialModel>, IUserCredentialRepository
{
	private readonly DatabaseContext _databaseContext;

	public UserCredentialRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
		_databaseContext = databaseContext;
	}
	public async Task<UserCredentialModel?> GetByUserName(string userName)
	{
		var userCredential = await _databaseContext.UserCredential.FirstOrDefaultAsync(u => u.UserName == userName);

		if (userCredential is not null)
		{
			return userCredential;
		}

		return null;
	}

	public async Task<UserCredentialModel> Register(UserCredentialModel userCredential)
	{
		await _databaseContext.UserProfile.AddAsync(userCredential.UserProfile);
		await _databaseContext.UserCredential.AddAsync(userCredential);
		await _databaseContext.SaveChangesAsync();

		return userCredential;
	}

	public async Task<UserCredentialModel> Login(UserCredentialModel userCredential)
	{
		var userCred = await _databaseContext.UserCredential.FirstOrDefaultAsync(u => u.UserName == userCredential.UserName);
		return userCred ?? new();
	}
}