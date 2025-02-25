using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ToDoApp.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
	private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
	private readonly ILocalStorageService _localStorageService;
	private readonly NavigationManager _navigationManager;

	public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, NavigationManager navigationManager)
	{
		_localStorageService = localStorageService;
		_navigationManager = navigationManager;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await _localStorageService.GetItemAsync<string>(Constants.AuthToken);

		if (string.IsNullOrWhiteSpace(token))
		{
			return await Task.FromResult(new AuthenticationState(_anonymous));
		}

		var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
		return await Task.FromResult(new AuthenticationState(user));
	}

	public async Task MarkUserAsAuthenticated(string token)
	{

		var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
		await _localStorageService.SetItemAsync(Constants.AuthToken, token);
		// Notify Blazor that the authentication state has changed
		NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
	}

	public async Task MarkUserAsLogout(bool forceLoad = false)
	{
		var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
		NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
		await _localStorageService.RemoveItemAsync(Constants.AuthToken);
		_navigationManager.NavigateTo("/", forceLoad);
	}


	private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
	{
		var claims = new List<Claim>();
		var payload = jwt.Split('.')[1];
		var jsonBytes = ParseBase64WithoutPadding(payload);
		var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

		keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

		if (roles != null)
		{
			if (roles.ToString().Trim().StartsWith("["))
			{
				var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

				foreach (var parsedRole in parsedRoles)
				{
					claims.Add(new Claim(ClaimTypes.Role, parsedRole));
				}
			}
			else
			{
				claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
			}

			keyValuePairs.Remove(ClaimTypes.Role);
		}

		claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

		return claims;
	}

	private byte[] ParseBase64WithoutPadding(string base64)
	{
		switch (base64.Length % 4)
		{
			case 2: base64 += "=="; break;
			case 3: base64 += "="; break;
		}
		return Convert.FromBase64String(base64);
	}
}