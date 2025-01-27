using Microsoft.AspNetCore.Components;
using System.Globalization;
using ToDoApp.Client.Authentication;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Components;

public partial class UserSettingsContent
{
	#region Parameters

	[Parameter] public UserProfileModel UserProfile { get; set; }

	#endregion Parameters

	#region Private Properties

	[Inject] private CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }

	[Parameter] public EventCallback OnEditProfile { get; set; }

	private string SelectedLanguage { get; set; }

	private string? UserName => $"{UserProfile?.FirstName} {UserProfile?.LastName}";

	#endregion Private Properties

	#region Private Methods

	protected override async Task OnParametersSetAsync()
	{
		SelectedLanguage = await LocalStorageService.GetItemAsync<string>("language");
		await base.OnParametersSetAsync();
	}

	private void EditProfile()
	{
		if (OnEditProfile.HasDelegate)
		{
			OnEditProfile.InvokeAsync();
		}

		NavigationManager?.NavigateTo(PageRoute.Profile);
	}

	private async Task ChangeLanguage(ChangeEventArgs e)
	{
		var language = e.Value?.ToString();
		await LocalStorageService.SetItemAsync("language", language);
		SelectedLanguage = await LocalStorageService.GetItemAsync<string>("language");
		CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(SelectedLanguage);
		CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(SelectedLanguage);
		NavigationManager?.NavigateTo(NavigationManager.Uri, forceLoad: true);
	}

	private async Task Logout()
	{
		await CustomAuthenticationStateProvider.MarkUserAsLogout();
	}

	#endregion Private Methods
}