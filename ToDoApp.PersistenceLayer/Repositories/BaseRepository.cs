using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using ToDoApp.Shared.Search;

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
		queryable = ApplyFilters(queryable, pageRequest.SearchFilters, pageRequest.FilterJunction);

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

	private IQueryable<T> ApplyFilters(IQueryable<T> query, List<SearchFilter> filters, FilterJunction filterJunction)
	{
		if (filters == null || !filters.Any()) return query;

		var parameter = Expression.Parameter(typeof(T), "x");
		Expression finalExpression = null;

		try
		{
			foreach (var filter in filters)
			{
				var property = Expression.Property(parameter, filter.PropertyName);

				object convertedValue = filter.PropertyValue;

				// Convert JsonElement to appropriate type
				if (filter.PropertyValue is JsonElement jsonElement)
				{
					if (property.Type == typeof(int))
					{
						convertedValue = jsonElement.GetInt32();
					}
					else if (property.Type == typeof(Guid))
					{
						convertedValue = jsonElement.GetGuid();
					}
					else if (property.Type == typeof(bool))
					{
						convertedValue = jsonElement.GetBoolean();
					}
					else if (property.Type == typeof(DateTime))
					{
						convertedValue = jsonElement.GetDateTime();
					}
					else if (property.Type == typeof(string))
					{
						convertedValue = jsonElement.GetString();
					}
				}

				var value = Expression.Constant(ChangeType(convertedValue, property.Type));

				Expression condition = filter.Operator switch
				{
					FilterOperator.Equal => Expression.Equal(property, value),
					FilterOperator.NotEqual => Expression.NotEqual(property, value),
					FilterOperator.GreaterThan => Expression.GreaterThan(property, value),
					FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, value),
					FilterOperator.LessThan => Expression.LessThan(property, value),
					FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, value),
					FilterOperator.Contains when property.Type == typeof(string) =>
						Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) }), value),
					FilterOperator.StartsWith when property.Type == typeof(string) =>
						Expression.Call(property, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), value),
					FilterOperator.EndsWith when property.Type == typeof(string) =>
						Expression.Call(property, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), value),
					_ => throw new NotSupportedException($"Operator {filter.Operator} is not supported")
				};

				switch (filterJunction)
				{
					case FilterJunction.AND:
						finalExpression = finalExpression is null ? condition : Expression.AndAlso(finalExpression, condition);
						break;
					case FilterJunction.OR:
						finalExpression = finalExpression is null ? condition : Expression.OrElse(finalExpression, condition);
						break;
				}
			}

			if (finalExpression is null) return query;

			var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
			return query.Where(lambda);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return query;
		}
	}

	public object ChangeType(object value, Type type)
	{
		if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
		if (value == null) return null;
		if (type == value.GetType()) return value;
		if (type.IsEnum)
		{
			if (value is string)
				return Enum.Parse(type, value as string);
			else
				return Enum.ToObject(type, value);
		}
		if (!type.IsInterface && type.IsGenericType)
		{
			Type innerType = type.GetGenericArguments()[0];
			object innerValue = ChangeType(value, innerType);
			return Activator.CreateInstance(type, new object[] { innerValue });
		}
		if (value is string && type == typeof(Guid)) return new Guid(value as string);
		if (value is string && type == typeof(Version)) return new Version(value as string);
		if (!(value is IConvertible)) return value;
		return Convert.ChangeType(value, type);
	}
}