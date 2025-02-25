using Blazored.LocalStorage;

namespace ToDoApp.Client.Authentication;

public class AuthTokenStore : IAuthTokenStore
{
	private readonly ILocalStorageService _localStorageService;

	public AuthTokenStore(ILocalStorageService localStorageService)
	{
		_localStorageService = localStorageService;
	}

	public async Task<string> GetToken()
	{
		var token = await _localStorageService.GetItemAsync<string>(Constants.AuthToken);

		if (!string.IsNullOrWhiteSpace(token))
		{
			return token;
		}

		return string.Empty;
	}

	public async Task RefreshToken(string token)
	{
		await _localStorageService.SetItemAsync(Constants.AuthToken, token);
	}
}