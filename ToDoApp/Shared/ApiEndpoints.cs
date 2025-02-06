namespace ToDoApp.Shared;

public static class ApiEndpoints
{
	public static class AuthEndpoints
	{
		public const string Login = "/api/auth/login";
		public const string Register = "/api/auth/register";
		public const string RefreshToken = "/api/auth/userCredential/{id}/refreshToken";
	}

	public static class TodoEndpoints
    {
        public const string GetAll = "/api/todos";
        public const string GetPaginatedResult = "/api/todos/paginatedResult";
		public const string GetById = "/api/todos/{id}";
        public const string Add = "/api/todos";
        public const string Update = "/api/todos/{id}";
        public const string Delete = "/api/todos/{id}";
		public const string Save = "/api/todos/save";
	}

	public static class UserProfileEndpoints
	{
		public const string GetById = "/api/userProfiles/{id}";
		public const string GetCurrentUserProfile = "/api/userProfiles/currentUserProfile";
		public const string Save = "/api/userProfiles/save";
	}

	public static class UserCredentialEndpoints
	{
		public const string GetById = "/api/userCredentials/{id}";
		public const string GetByUserName = "/api/userCredentials";
	}
}