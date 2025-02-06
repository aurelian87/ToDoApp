using ToDoApp.PersistenceLayer.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.PersistenceLayer.Repositories;

public class TodoRepository : BaseRepository<TodoModel>, ITodoRepository
{
	public TodoRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}
}