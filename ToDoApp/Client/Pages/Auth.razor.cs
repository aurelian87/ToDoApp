using Blazored.LocalStorage;
using ChangeTracking;
using Microsoft.AspNetCore.Components;
using ToDoApp.Client.Authentication;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Validations;

namespace ToDoApp.Client.Pages;

[Route($"{PageRoute.Auth}")]
public partial class Auth
{
	#region Fields

	private FluentValidationValidator? _fluentValidationValidator;
	private AuthModel _model;

	#endregion //Fields

	#region Private Properties

	[Inject] private IUserCredentialService? UserCredentialService { get; set; }

	[Inject] private IAuthService? AuthService { get; set; }

	[Inject] private CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }

	[Inject] private NavigationManager NavigationManager { get; set; }

	[Inject] private ILocalStorageService LocalStorageService { get; set; }

	private AuthModel Model
	{
		get
		{
			if (_model is null)
			{
				Model = new();
			}

			return _model!;
		}

		set
		{
			_model = value;

			if (_model is not IChangeTrackable)
			{
				_model = _model.AsTrackable();
			}
		}
	}

	private bool ShowRegister { get; set; }

	private bool ShowPassword { get; set; }

	#endregion Private Properties

	#region Private Methods

	private async Task Login()
	{
		var result = await AuthService!.Login(Model);

		if (string.IsNullOrWhiteSpace(result.Content))
		{
			return;
		}

		await CustomAuthenticationStateProvider.MarkUserAsAuthenticated(result.Content);
		NavigationManager.NavigateTo("/");
	}

	private async Task Register()
	{
		var result = await AuthService!.Register(Model);

		if (result.IsSuccessStatusCode)
		{
			ShowRegister = false;
			Model = new();
			return;
		}

		//if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
		//{
		//	Console.WriteLine($"Register Error: user is already used!");
		//}

		//RegisterErrors.Add("User is already used!");
		Console.WriteLine($"Register Error: {result.Error.ToString()}");
		Console.WriteLine($"Register Error: {result.Content}");
		Console.WriteLine($"Register Error: {result?.Content?.ToString()}");
		Console.WriteLine($"Register Error: {result?.ReasonPhrase}");
	}

	private void TooglePasswordVisibility()
	{
		ShowPassword = !ShowPassword;
	}

	private void CreateAccount()
	{
		ShowRegister = true;
		ShowPassword = false;
		Model = new();
	}

	private void GoToLogin()
	{
		ShowRegister = false;
		ShowPassword = false;
		Model = new();
	}

	#endregion Private Methods
}