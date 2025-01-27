using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using Refit;
using ToDoApp.Shared;

namespace ToDoApp.Client.Services;

public interface ITodoService
{
    private const string paginatedResultQuery = "?pageNumber={pageRequest.PageNumber}&pageSize={pageRequest.PageSize}&orderBy={pageRequest.OrderBy}";

    [Get(ApiEndpoints.TodoEndpoints.GetAll)]
    Task<List<TodoModel>> GetAll();

    [Get($"{ApiEndpoints.TodoEndpoints.GetPaginatedResult}{paginatedResultQuery}")]
    Task<PageResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest);

    [Get(ApiEndpoints.TodoEndpoints.GetById)]
    Task<TodoModel> GetById(int id);

    [Post(ApiEndpoints.TodoEndpoints.Add)]
    Task<TodoModel> Add(TodoModel todo);

    [Put(ApiEndpoints.TodoEndpoints.Update)]
    Task<TodoModel> Update(int id, TodoModel todo);

    [Delete(ApiEndpoints.TodoEndpoints.Delete)]
    Task<TodoModel> Delete(int id);

	[Post(ApiEndpoints.TodoEndpoints.Save)]
	Task<TodoModel> Save(TodoModel model);
}