using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using Refit;
using ToDoApp.Shared;

namespace ToDoApp.Client.Services;

public interface ITodoService
{
    [Get(ApiEndpoints.TodoEndpoints.GetAll)]
    Task<List<TodoModel>> GetAll();

    [Post(ApiEndpoints.TodoEndpoints.GetPaginatedResult)]
	Task<PageResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest);

	[Get(ApiEndpoints.TodoEndpoints.GetById)]
    Task<TodoModel> GetById(int id);

    [Delete(ApiEndpoints.TodoEndpoints.Delete)]
    Task<TodoModel> Delete(int id);

	[Post(ApiEndpoints.TodoEndpoints.Save)]
	Task<TodoModel> Save(TodoModel model);
}