using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Services
{
	public interface IToDoService
	{
        Task<List<ToDoModel>> GetAll();

        Task<ToDoModel> GetById(int id);

        Task<ToDoModel> Add(ToDoModel todo);

        Task<ToDoModel> Update(int id, ToDoModel todo);

        Task<ToDoModel> Delete(int id);
    }
}