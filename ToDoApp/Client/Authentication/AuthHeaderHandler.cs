using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ToDoApp.Client.Services;

namespace ToDoApp.Client.Authentication;

public class AuthHeaderHandler : DelegatingHandler
{
	private readonly IAuthTokenStore _authTokenStore;
	private readonly IAuthService _authService;
	private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

	public AuthHeaderHandler(IAuthTokenStore authTokenStore,
		IAuthService authService,
		CustomAuthenticationStateProvider customAuthenticationStateProvider)
	{
		_authTokenStore = authTokenStore;
		_authService = authService;
		_customAuthenticationStateProvider = customAuthenticationStateProvider;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = await _authTokenStore.GetToken();

		//potentially refresh token here if it has expired etc.
		if (!string.IsNullOrEmpty(token))
		{
			// Decode the JWT token
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			// Extract the expiration time
			var expirationTime = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

			// Optionally, you can convert the expiration time to a DateTime
			if (expirationTime is not null && long.TryParse(expirationTime, out var exp))
			{
				var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
				var utcNow = DateTime.UtcNow;
				// You can now use expirationDateTime to check if the token is expired
				Console.WriteLine($"AuthHeaderHandler SendAsync Token expires at: {expirationDateTime}");
				Console.WriteLine($"AuthHeaderHandler SendAsync DateTime.UtcNow: {utcNow}");

				if (expirationDateTime < utcNow)
				{
					Console.WriteLine($"AuthHeaderHandler SendAsync token has expired and should be logged out");
					await _customAuthenticationStateProvider.MarkUserAsLogout(true);
				}
				else if (expirationDateTime.AddSeconds(-10) < utcNow)
				{
					Console.WriteLine($"AuthHeaderHandler SendAsync token should be refreshed");
					//var serviceScope = _serviceProvider.CreateScope();
					//var authService = serviceScope.ServiceProvider.GetRequiredService<IAuthService>();
					var userCredentialIdString = jwtToken.Claims.FirstOrDefault(c => c.Type == "userCredentialId")?.Value;
					var userCredentialId = int.Parse(userCredentialIdString);
					var result = await _authService.RefreshToken(userCredentialId);
					token = result.Content;
					await _authTokenStore.RefreshToken(token);
				}
				else
				{
					Console.WriteLine($"AuthHeaderHandler SendAsync token is still valid");
				}
			}
		}

		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}
}