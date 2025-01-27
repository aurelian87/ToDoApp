using Refit;
using ToDoApp.Shared;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Services;

public interface IAuthService
{
	[Post(ApiEndpoints.AuthEndpoints.Login)]
	Task<ApiResponse<string?>> Login(AuthModel model);

	[Post(ApiEndpoints.AuthEndpoints.Register)]
	Task<ApiResponse<UserCredentialModel?>> Register(AuthModel model);

	[Post(ApiEndpoints.AuthEndpoints.RefreshToken)]
	Task<ApiResponse<string?>> RefreshToken(int id);
}