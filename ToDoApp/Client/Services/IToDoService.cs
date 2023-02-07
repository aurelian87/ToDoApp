using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Services
{
	public interface IToDoService
	{
        Task<IEnumerable<ToDoModel>> GetToDos();

        Task<List<ToDoModel>> GetAll();

        Task<ToDoModel> GetById();
    }
}