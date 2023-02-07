using System.Net.Http.Json;
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

        public async Task<List<ToDoModel>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");
        }

        public async Task<ToDoModel> GetById()
        {
            return await _httpClient.GetFromJsonAsync<ToDoModel>("/api/ToDos/29");
        }
    }
}