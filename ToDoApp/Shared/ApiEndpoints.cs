namespace ToDoApp.Shared;

public static class ApiEndpoints
{
    public static class TodoEndpoints
    {
        public const string GetAll = "/api/todos";
        public const string GetPaginatedResult = "/api/todos/paginatedResult";
        public const string GetById = "/api/todos/{id}";
        public const string Add = "/api/todos";
        public const string Update = "/api/todos/{id}";
        public const string Delete = "/api/todos/{id}";
    }
}