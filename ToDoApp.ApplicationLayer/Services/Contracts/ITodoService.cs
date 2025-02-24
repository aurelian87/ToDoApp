using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.ApplicationLayer.Services.Contracts;

public interface ITodoService
{
	Task<List<TodoModel>> GetAll();

	Task<PaginatedResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest, int userProfileId);

	Task<TodoModel?> GetById(int id);

	Task<TodoModel> Add(TodoModel todo);

	Task<TodoModel?> Update(int id, TodoModel todo);

	Task<TodoModel?> Delete(int id);

	Task<TodoModel> Save(TodoModel model);
}