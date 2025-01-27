﻿using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using ToDoApp.ApplicationLayer.Services.Contracts;

namespace ToDoApp.ApplicationLayer.Services;

public class TodoService : ITodoService
{
	private readonly ITodoRepository _toDoRepository;

	public TodoService(ITodoRepository toDoRepository)
	{
		_toDoRepository = toDoRepository;
	}

	public async Task<List<TodoModel>> GetAll()
	{
		return await _toDoRepository.GetAll();
	}

	public async Task<PageResponse<TodoModel>> GetPaginatedResult(PageRequest pageRequest, int userProfileId)
	{
		return await _toDoRepository.GetPaginatedResult(pageRequest, userProfileId);
	}

	public async Task<TodoModel?> GetById(int id)
	{
		return await _toDoRepository.GetById(id);
	}

	public async Task<TodoModel> Add(TodoModel todo)
	{
		return await _toDoRepository.Add(todo);
	}

	public async Task<TodoModel?> Update(int id, TodoModel todo)
	{
		return await _toDoRepository.Update(id, todo);
	}

	public async Task<TodoModel?> Delete(int id)
	{
		return await _toDoRepository.Delete(id);
	}

	public async Task<TodoModel> Save(TodoModel model)
	{
		return await _toDoRepository.Save(model);
	}
}