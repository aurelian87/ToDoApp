using System.Net.Http.Json;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.RequestsUri;
using System.Text.Json;
using ToDoApp.Shared.Response;
using ToDoApp.Shared.Requests;

namespace ToDoApp.Client.Services
{
    public class ToDoService : IToDoService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private const string requestUri = TodoRequestsUri.GetAll;

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

        public async Task<PageResponse<ToDoModel>> GetPaginatedResult(PageRequest pageRequest)
        {
            var result = new PageResponse<ToDoModel>();

			var queryUri = $"{requestUri}/paginatedResult?pageNumber={pageRequest.PageNumber}&pageSize={pageRequest.PageSize}&orderBy={pageRequest.OrderBy}";
            var response =  await _httpClient.GetAsync(queryUri);

            if (response.IsSuccessStatusCode) 
            {
                var responseString = await response.Content.ReadAsStringAsync();
				result = JsonSerializer.Deserialize<PageResponse<ToDoModel>>(responseString,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }

			return result!;
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