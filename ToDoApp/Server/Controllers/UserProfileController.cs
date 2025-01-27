using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToDoApp.ApplicationLayer.Services.Contracts;
using System.Security.Claims;
using ToDoApp.Shared;
using ToDoApp.Shared.Models;

namespace ToDoApp.Server.Controllers;

[Authorize]
[ApiController]
public class UserProfileController : Controller
{
	private readonly IUserProfileService _userProfileService;

	public UserProfileController(IUserProfileService userProfileService)
	{
		_userProfileService = userProfileService;
	}

	[HttpGet]
	[Route(ApiEndpoints.UserProfileEndpoints.GetById)]
	public async Task<IActionResult> GetById([FromRoute] int id)
	{
		var result = await _userProfileService.GetById(id);

		if (result is not null)
		{
			return Ok(result);
		}

		return NotFound();
	}

	[HttpGet]
	[Route(ApiEndpoints.UserProfileEndpoints.GetCurrentUserProfile)]
	public async Task<IActionResult> GetCurrentUserProfile()
	{
		var userProfileId = 0;

		if (User is ClaimsPrincipal && User.Identity.IsAuthenticated)
		{
			var claimsPrincipal = User;
			var userProfileIdString = claimsPrincipal.FindFirstValue("userProfileId");
			userProfileId = int.Parse(userProfileIdString);
		}

		var result = await _userProfileService.GetById(userProfileId);

		if (result is not null)
		{
			return Ok(result);
		}

		return NotFound();
	}


	[HttpPost]
	[Route(ApiEndpoints.UserProfileEndpoints.Save)]
	public async Task<IActionResult> Save(UserProfileModel userProfile)
	{
		var result = await _userProfileService.Save(userProfile);
		return Ok(result);
	}
}