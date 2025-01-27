using Microsoft.EntityFrameworkCore;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using System.Linq.Dynamic.Core;

namespace ToDoApp.PersistenceLayer.Repositories;

public class TodoRepository : BaseRepository<TodoModel>, ITodoRepository
{

	private readonly DatabaseContext _databaseContext;

	public TodoRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public async Task<PageResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest, int userProfileId)
	{
		var dbSet = _databaseContext.Set<TodoModel>();

		// Apply the filter
		var queryable = dbSet.AsQueryable();
		queryable = queryable.Where(i => i.UserProfileId == userProfileId);

		// Apply ordering if specified
		if (!string.IsNullOrWhiteSpace(pageRequest.OrderBy))
		{
			queryable = queryable.OrderBy(pageRequest.OrderBy);
		}

		// Apply pagination
		var result = await queryable
						  .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
						  .Take(pageRequest.PageSize)
						  .ToListAsync();

		var response = new PageResponse<TodoModel>
		{
			Data = result,
			PageNumber = pageRequest.PageNumber,
			PageSize = pageRequest.PageSize,
			TotalItems = await queryable.CountAsync()
		};

		return response;
	}

	public async Task<List<TodoModel>> GetAllByUserProfileId(int userProfileId)
	{
		var dbSet = _databaseContext.Set<TodoModel>();
		var result = await dbSet.ToListAsync();
		result = result.FindAll(x => x.UserProfileId == userProfileId);
		return result;
	}
}