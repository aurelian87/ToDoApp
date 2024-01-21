using Microsoft.EntityFrameworkCore;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using System.Linq.Dynamic.Core;

namespace ToDoApp.PersistenceLayer.Repositories
{
	public class ToDoRepository : IToDoRepository
	{

		private readonly DatabaseContext _databaseContext;

		public ToDoRepository(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public async Task<List<ToDoModel>> GetAll()
		{
			return await _databaseContext.ToDos.ToListAsync();
		}

		public async Task<PageResponse<ToDoModel>> GetPaginatedResult(PageRequest pageRequest)
		{
			var result = new List<ToDoModel>();

			if (!string.IsNullOrEmpty(pageRequest.OrderBy))
			{
				result = await _databaseContext.ToDos
							  .OrderBy(pageRequest.OrderBy)
							  .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
							  .Take(pageRequest.PageSize)
							  .ToListAsync();
			}
			else
			{
				result = await _databaseContext.ToDos
							  .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
							  .Take(pageRequest.PageSize)
							  .ToListAsync();
			}

			var response = new PageResponse<ToDoModel>
			{
				Data = result,
				PageNumber = pageRequest.PageNumber,
				PageSize = pageRequest.PageSize,
				TotalItems = _databaseContext.ToDos.Count()
			};

			return response;
		}

		public async Task<ToDoModel?> GetById(int id)
		{
			var todo =  await _databaseContext.ToDos.FindAsync(id);

			if (todo is not null)
			{
				return todo;
			}

			return null;
		}

		public async Task<ToDoModel> Add(ToDoModel todo)
		{
			var item = new ToDoModel 
			{
				Title = todo.Title,
				Description = todo.Description,
				DueDate = new DateTime(todo.DueDate.Year, todo.DueDate.Month, todo.DueDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
			};

			await _databaseContext.ToDos.AddAsync(item);
			await _databaseContext.SaveChangesAsync();

			return item;
		}

		public async Task<ToDoModel?> Update(int id, ToDoModel todo)
		{
			var item = _databaseContext.ToDos.Find(id);

			if (item is not null)
			{
				item.Title = todo.Title;
				item.Description = todo.Description;
				item.DueDate = new DateTime(todo.DueDate.Year, todo.DueDate.Month, todo.DueDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

				await _databaseContext.SaveChangesAsync();
				return item;
			}

			return null;
		}

		public async Task<ToDoModel?> Delete(int id)
		{
			var todo = _databaseContext.ToDos.Find(id);

			if (todo is not null)
			{
				_databaseContext.ToDos.Remove(todo);
				await _databaseContext.SaveChangesAsync();
				return todo;
			}

			return null;
		}
	}
}