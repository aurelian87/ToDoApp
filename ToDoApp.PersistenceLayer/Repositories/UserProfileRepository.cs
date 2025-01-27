using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.PersistenceLayer.Repositories;

public class UserProfileRepository : BaseRepository<UserProfileModel>, IUserProfileRepository
{
	public UserProfileRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}
}