using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.Shared.Repositories;

public interface IBaseRepository<T> where T : class
{
	Task<List<T>> GetAll();

	Task<T?> GetById(int id);

	Task<T> Add(T model);

	Task<T?> Update(int id, T model);

	Task<T?> Delete(int id);

	Task<T> Save(T model);

	Task<PageResponse<T>> GetPaginatedResult(PageRequest pageRequest);
}