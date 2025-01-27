using ToDoApp.Shared.Models;

namespace ToDoApp.ApplicationLayer.Services.Contracts;

public interface IUserProfileService
{
	Task<UserProfileModel?> GetById(int id);

	Task<UserProfileModel?> GetCurrentUserProfile(int id);

	Task<UserProfileModel?> Save(UserProfileModel userProfile);
}