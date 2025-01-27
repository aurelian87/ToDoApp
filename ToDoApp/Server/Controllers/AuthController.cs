using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared;
using ToDoApp.Shared.Models;

namespace ToDoApp.Server.Controllers;

[ApiController]
public class AuthController : Controller
{
	private readonly IAuthService _authService;
	private readonly IUserCredentialService _userCredentialService;

	public AuthController(IAuthService authService, IUserCredentialService userCredentialService)
	{
		_authService = authService;
		_userCredentialService = userCredentialService;
	}

	[HttpPost]
	[Route(ApiEndpoints.AuthEndpoints.Register)]
	public async Task<IActionResult> Register(AuthModel model)
	{
		var existUserCredential = await _userCredentialService.GetByUserName(model.UserName);

		if (existUserCredential is not null)
		{
			return BadRequest("User already exists");
		}

		var result = await _authService.Register(model);
		return Ok(result);
	}

	[HttpPost]
	[Route(ApiEndpoints.AuthEndpoints.Login)]
	public async Task<IActionResult> Login(AuthModel model)
	{
		var existUserCredential = await _userCredentialService.GetByUserName(model.UserName);

		if (existUserCredential is null)
		{
			return NotFound("User not found!");
		}

		if (existUserCredential is not null && existUserCredential.Password != model.Password)
		{
			return NotFound("Wrong password!");
		}

		var result = await _authService.Login(model);
		var token = CreateToken(result);
		return Ok(token);
	}

	[HttpPost]
	[Route(ApiEndpoints.AuthEndpoints.RefreshToken)]
	public async Task<IActionResult> RefreshToken([FromRoute] int id)
	{
		var result = await _userCredentialService.GetById(id);
		var token = CreateToken(result);
		return Ok(token);
	}

	private string CreateToken(UserCredentialModel userCredential)
	{
		List<Claim> claims = new()
		{
			new Claim("userCredentialId", $"{userCredential.Id}"),
			new Claim("userName", $"{userCredential.UserName}"),
			new Claim("userProfileId", $"{userCredential.UserProfileId}"),
			new Claim("role", "AdminRole"),
			new Claim("role", "TestRole")
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my super secret"));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken(
			issuer: "https://localhost:7217/",
			audience: "https://localhost:7217/",
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(60),
			signingCredentials: creds
		);

		var jwt = new JwtSecurityTokenHandler().WriteToken(token);

		return jwt;
	}
}