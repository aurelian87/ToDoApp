using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using Refit;
using ToDoApp.Shared;

namespace ToDoApp.Client.Services
{
	public interface IToDoService
	{
        private const string paginatedResultQuery = "?pageNumber={pageRequest.PageNumber}&pageSize={pageRequest.PageSize}&orderBy={pageRequest.OrderBy}";

        [Get(ApiEndpoints.TodoEndpoints.GetAll)]
        Task<List<ToDoModel>> GetAll();

        [Get($"{ApiEndpoints.TodoEndpoints.GetPaginatedResult}{paginatedResultQuery}")]
        Task<PageResponse<ToDoModel>> GetPaginatedResult(PageRequest pageRequest);

        [Get(ApiEndpoints.TodoEndpoints.GetById)]
        Task<ToDoModel> GetById(int id);

        [Post(ApiEndpoints.TodoEndpoints.Add)]
        Task<ToDoModel> Add(ToDoModel todo);

        [Put(ApiEndpoints.TodoEndpoints.Update)]
        Task<ToDoModel> Update(int id, ToDoModel todo);

        [Delete(ApiEndpoints.TodoEndpoints.Delete)]
        Task<ToDoModel> Delete(int id);
    }
}