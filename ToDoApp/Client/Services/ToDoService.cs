using System.Net.Http.Json;
using System.Reflection;
using ToDoApp.Client.Pages;
using ToDoApp.Shared.Models;
using static System.Net.WebRequestMethods;

namespace ToDoApp.Client.Services
{
    public class ToDoService : IToDoService
    {
        private readonly HttpClient _httpClient;

        public ToDoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ToDoModel>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos") ?? new();
        }

        public async Task<ToDoModel> GetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<ToDoModel>($"/api/ToDos/{id}") ?? new();
		}

        public async Task<ToDoModel> Add(ToDoModel todo)
        {
            await _httpClient.PostAsJsonAsync("/api/ToDos", todo);
            return todo;
        }

        public async Task<ToDoModel> Update(int id,ToDoModel todo)
        {
            await _httpClient.PutAsJsonAsync($"/api/ToDos/{id}", todo);
            return todo;
        }

        public async Task<ToDoModel> Delete(int id)
        {
            await _httpClient.DeleteAsync($"/api/ToDos/{id}");
            return new(); 
        }
    }
}