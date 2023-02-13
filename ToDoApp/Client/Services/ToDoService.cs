using System.Net.Http.Json;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Services
{
    public class ToDoService : IToDoService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private const string requestUri = "/api/ToDos";

        #endregion //Fields

        #region Constructors

        public ToDoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion //Constructors

        #region Public Methods

        public async Task<List<ToDoModel>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<ToDoModel>>(requestUri) ?? new();
        }

        public async Task<ToDoModel> GetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<ToDoModel>($"{requestUri}/{id}") ?? new();
        }

        public async Task<ToDoModel> Add(ToDoModel todo)
        {
            await _httpClient.PostAsJsonAsync(requestUri, todo);
            return todo;
        }

        public async Task<ToDoModel> Update(int id, ToDoModel todo)
        {
            await _httpClient.PutAsJsonAsync($"{requestUri}/{id}", todo);
            return todo;
        }

        public async Task<ToDoModel> Delete(int id)
        {
            await _httpClient.DeleteAsync($"{requestUri}/{id}");
            return new();
        }

        #endregion //Public Methods
    }
}