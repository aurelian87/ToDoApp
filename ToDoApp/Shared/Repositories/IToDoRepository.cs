using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.Shared.Repositories;

public interface ITodoRepository : IBaseRepository<TodoModel>
{
	Task<PageResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest, int userProfileId);

	Task<List<TodoModel>> GetAllByUserProfileId(int userProfileId);
}