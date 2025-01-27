using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;

namespace ToDoApp.Server.Controllers;

[Authorize]
[ApiController]
//[Route(TodoRequestsUri.GetAll)]
public class TodoController : Controller
{
	private readonly ITodoService _toDoService;

	public TodoController(ITodoService toDoService)
	{
		_toDoService = toDoService;
	}

	[HttpGet]
	[Route(ApiEndpoints.TodoEndpoints.GetAll)]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _toDoService.GetAll());
	}

	[HttpGet]
	//[Route("paginatedResult")]
	[Route(ApiEndpoints.TodoEndpoints.GetPaginatedResult)]
	public async Task<IActionResult> GetPaginatedResult([FromQuery] PageRequest pageRequest)
	{
		var userProfileId = 0;

		if (User is ClaimsPrincipal && User.Identity.IsAuthenticated)
		{
			var claimsPrincipal = User;
			var userProfileIdString = claimsPrincipal.FindFirstValue("userProfileId");
			userProfileId = int.Parse(userProfileIdString);
		}

		return Ok(await _toDoService.GetPaginatedResult(pageRequest, userProfileId));
	}

	[HttpGet]
	//[Route("{id}")]
	[Route(ApiEndpoints.TodoEndpoints.GetById)]
	public async Task<IActionResult> GetById([FromRoute] int id)
	{
		var result = await _toDoService.GetById(id);

		if (result is not null)
		{
			return Ok(result);
		}

		return NotFound();
	}

	[HttpPost]
	[Route(ApiEndpoints.TodoEndpoints.Add)]
	public async Task<IActionResult> Add(TodoModel todo)
	{
		//var userProfileId = 0;

		//if (User is ClaimsPrincipal && User.Identity.IsAuthenticated)
		//{
		//	var claimsPrincipal = User;
		//	var userProfileIdString = claimsPrincipal.FindFirstValue("userProfileId");
		//	userProfileId = int.Parse(userProfileIdString);
		//}

		//todo.UserProfileId = userProfileId;
		return Ok(await _toDoService.Add(todo));
	}

	[HttpPut]
	//[Route("{id}")]
	[Route(ApiEndpoints.TodoEndpoints.Update)]
	public async Task<IActionResult> Update([FromRoute] int id, TodoModel todo)
	{
		var result = await _toDoService.Update(id, todo);

		if (result is not null)
		{
			return Ok(result);
		}

		return NotFound();
	}

	[HttpDelete]
	//[Route("{id}")]
	[Route(ApiEndpoints.TodoEndpoints.Delete)]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		var result = await _toDoService.Delete(id);

		if (result is not null)
		{
			return Ok(result);
		}

		return NotFound();
	}

	[HttpPost]
	[Route(ApiEndpoints.TodoEndpoints.Save)]
	public async Task<IActionResult> Save(TodoModel model)
	{
		var userProfileId = 0;

		if (User is ClaimsPrincipal && User.Identity.IsAuthenticated)
		{
			var claimsPrincipal = User;
			var userProfileIdString = claimsPrincipal.FindFirstValue("userProfileId");
			userProfileId = int.Parse(userProfileIdString);
		}

		model.UserProfileId = userProfileId;

		return Ok(await _toDoService.Save(model));
	}
}