using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.PersistenceLayer.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
	private readonly DatabaseContext _databaseContext;

	public BaseRepository(DatabaseContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public virtual async Task<List<T>> GetAll()
	{
		var dbSet = _databaseContext.Set<T>();
		var result = await dbSet.ToListAsync();
		return result;
	}

	public virtual async Task<T?> GetById(int id)
	{
		var dbSet = _databaseContext.Set<T>();
		var result = await dbSet.FindAsync(id);
		return result;
	}

	public virtual async Task<T> Add(T model)
	{
		var dbSet = _databaseContext.Set<T>();
		var result = await dbSet.AddAsync(model);
		await _databaseContext.SaveChangesAsync();
		return result.Entity;
	}

	public virtual async Task<T?> Update(int id, T model)
	{
		var dbSet = _databaseContext.Set<T>();
		var item = await dbSet.FindAsync(id);

		if (item is not null)
		{
			_databaseContext.Entry(item).CurrentValues.SetValues(model);
			await _databaseContext.SaveChangesAsync();
			return item;
		}

		return null;
	}

	public virtual async Task<T?> Delete(int id)
	{
		var dbSet = _databaseContext.Set<T>();
		var item = await dbSet.FindAsync(id);

		if (item is not null)
		{
			_databaseContext.Remove(item);
			await _databaseContext.SaveChangesAsync();

			return item;
		}

		return null;
	}

	public virtual async Task<T> Save(T model)
	{
		var dbSet = _databaseContext.Set<T>();
		var primaryKey = dbSet.EntityType.FindPrimaryKey();

		if (primaryKey is null)
		{
			throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a primary key defined.");
		}

		var keyValues = primaryKey.Properties
			.Select(p => p?.PropertyInfo?.GetValue(model))
			.ToArray();

		var item = await dbSet.FindAsync(keyValues);

		if (item is not null)
		{
			_databaseContext.Entry(item).CurrentValues.SetValues(model);
			await _databaseContext.SaveChangesAsync();
			return item;
		}
		else
		{
			var result = await dbSet.AddAsync(model);
			await _databaseContext.SaveChangesAsync();
			return result.Entity;
		}
	}

	public virtual async Task<PageResponse<T>> GetPaginatedResult(PageRequest pageRequest)
	{
		var dbSet = _databaseContext.Set<T>();

		// Apply the filter
		var queryable = dbSet.AsQueryable();

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

		var response = new PageResponse<T>
		{
			Data = result,
			PageNumber = pageRequest.PageNumber,
			PageSize = pageRequest.PageSize,
			TotalItems = await queryable.CountAsync()
		};

		return response;
	}
}