using Microsoft.AspNetCore.Mvc;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared;

namespace ToDoApp.Server.Controllers;

[ApiController]
public class UserCredentialController : Controller
{
	private readonly IUserCredentialService _userCredentialService;

	public UserCredentialController(IUserCredentialService userCredentialService)
	{
		_userCredentialService = userCredentialService;
	}

	[HttpGet]
	[Route(ApiEndpoints.UserCredentialEndpoints.GetById)]
	public async Task<IActionResult> GetById([FromRoute] int id)
	{
		var result = await _userCredentialService.GetById(id);

		if (result != null)
		{
			return Ok(result);
		}

		return NotFound();
	}

	[HttpGet]
	[Route(ApiEndpoints.UserCredentialEndpoints.GetByUserName)]
	public async Task<IActionResult> GetByUserName([FromQuery] string userName)
	{
		var result = await _userCredentialService.GetByUserName(userName);

		if (result != null)
		{
			return Ok(result);
		}

		return NotFound();
	}
}