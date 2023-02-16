namespace ToDoApp.Shared.RequestsUri
{
    public class TodoRequestsUri
    {
        public const string GetAll = "/api/todos";
        public const string GetById = "/api/todos/{id:int}";
        public const string Update = "/api/todos/{id}";
        public const string Delete = "/api/todos/{id:int}";


        public static string CreateUpdate(int id)
        {
            return Update.Replace("{id}", id.ToString());
        }
    }
}