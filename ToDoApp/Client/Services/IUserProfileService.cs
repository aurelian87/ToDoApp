using Refit;
using ToDoApp.Shared.Models;
using ToDoApp.Shared;

namespace ToDoApp.Client.Services;

public interface IUserProfileService
{
	[Get(ApiEndpoints.UserProfileEndpoints.GetById)]
	Task<ApiResponse<UserProfileModel?>> GetById(int id);

	[Get(ApiEndpoints.UserProfileEndpoints.GetCurrentUserProfile)]
	Task<ApiResponse<UserProfileModel?>> GetCurrentUserProfile();

	[Post(ApiEndpoints.UserProfileEndpoints.Save)]
	Task<ApiResponse<UserProfileModel?>> Save(UserProfileModel userProfile);
}