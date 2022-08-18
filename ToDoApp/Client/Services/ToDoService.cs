using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Services
{
	public class ToDoService : IToDoService
	{
		private readonly HttpClient _httpClient;

		public ToDoService(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

		public Task<IEnumerable<ToDoModel>> GetToDos()
		{
			throw new NotImplementedException();
		}
	}
}