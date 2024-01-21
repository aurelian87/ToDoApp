using Microsoft.AspNetCore.Mvc;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.RequestsUri;
using ToDoApp.ApplicationLayer.Services;
using ToDoApp.Shared;

namespace ToDoApp.Server.Controllers
{
	[ApiController]
	//[Route(TodoRequestsUri.GetAll)]
	public class ToDosController : Controller
	{
		private readonly IToDoService _toDoService;

		public ToDosController(IToDoService toDoService)
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
			return Ok(await _toDoService.GetPaginatedResult(pageRequest));
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
        public async Task<IActionResult> Add(ToDoModel todo)
		{
			return Ok(await _toDoService.Add(todo));
		}

		[HttpPut]
        //[Route("{id}")]
        [Route(ApiEndpoints.TodoEndpoints.Update)]
        public async Task<IActionResult> Update([FromRoute] int id, ToDoModel todo)
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
	}
}