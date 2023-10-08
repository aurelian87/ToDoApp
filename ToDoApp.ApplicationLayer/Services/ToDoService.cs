using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.ApplicationLayer.Services
{
	public class ToDoService : IToDoService
	{
		private readonly IToDoRepository _toDoRepository;

		public ToDoService(IToDoRepository toDoRepository)
		{
			_toDoRepository = toDoRepository;
		}

		public async Task<List<ToDoModel>> GetAll()
		{
			return await _toDoRepository.GetAll();
		}

		public async Task<PageResponse<ToDoModel>> GetPaginatedResult(PageRequest pageRequest)
		{
			return await _toDoRepository.GetPaginatedResult(pageRequest);
		}

		public async Task<ToDoModel?> GetById(int id)
		{
			return await _toDoRepository.GetById(id);
		}

		public async Task<ToDoModel> Add(ToDoModel todo)
		{
			return await _toDoRepository.Add(todo);
		}

		public async Task<ToDoModel?> Update(int id, ToDoModel todo)
		{
			return await _toDoRepository.Update(id, todo);
		}

		public async Task<ToDoModel?> Delete(int id)
		{
			return await _toDoRepository.Delete(id);
		}
	}
}