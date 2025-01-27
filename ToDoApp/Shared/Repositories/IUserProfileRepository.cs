using ToDoApp.Shared.Models;

namespace ToDoApp.Shared.Repositories;

public interface IUserProfileRepository : IBaseRepository<UserProfileModel>
{
	//Task<UserProfileModel?> GetCurrentUserProfile(int id);
}