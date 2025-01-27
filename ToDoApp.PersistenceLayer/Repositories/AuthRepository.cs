using Microsoft.EntityFrameworkCore;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.PersistenceLayer.Repositories;

public class AuthRepository : BaseRepository<AuthModel>, IAuthRepository
{
	private readonly DatabaseContext _databaseContext;

	public AuthRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public async Task<UserCredentialModel> Register(AuthModel model)
	{
		var userCredential = new UserCredentialModel
		{
			UserName = model.UserName,
			Password = model.Password,
			UserProfile = new()
		};

		await _databaseContext.UserProfile.AddAsync(userCredential.UserProfile);
		await _databaseContext.UserCredential.AddAsync(userCredential);
		await _databaseContext.SaveChangesAsync();

		return userCredential;
	}

	public async Task<UserCredentialModel> Login(AuthModel model)
	{
		var userCred = await _databaseContext.UserCredential.FirstOrDefaultAsync(u => u.UserName == model.UserName);
		return userCred ?? new();
	}
}