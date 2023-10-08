using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.ApplicationLayer.Services;

public interface IToDoService
{
    Task<List<ToDoModel>> GetAll();

    Task<PageResponse<ToDoModel>> GetPaginatedResult(PageRequest pageRequest);

    Task<ToDoModel?> GetById(int id);

    Task<ToDoModel> Add(ToDoModel todo);

    Task<ToDoModel?> Update(int id, ToDoModel todo);

    Task<ToDoModel?> Delete(int id);
}