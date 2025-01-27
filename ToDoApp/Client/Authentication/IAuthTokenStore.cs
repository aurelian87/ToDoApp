namespace ToDoApp.Client.Authentication;

public interface IAuthTokenStore
{
	Task<string> GetToken();

	Task RefreshToken(string token);
}