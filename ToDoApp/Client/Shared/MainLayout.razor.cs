using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ToDoApp.Client.Components;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Shared;

public partial class MainLayout
{
	#region Fields

	private UserSettingsContent? _userSettingsContent;

	#endregion Fields

	#region Public Methods

	public void ShowPageLoader()
	{
		ShowLoader = true;
		StateHasChanged();
	}

	public void HidePageLoader()
	{
		ShowLoader = false;
		StateHasChanged();
	}

	public Task RefreshUserSettings(UserProfileModel model)
	{
		return LoadCurrentUserProfile(model);
	}

	#endregion Public Methods

	#region Private Properties

	[Inject] private IUserProfileService UserProfileService { get; set; }

	private AuthenticationState AuthState { get; set; }

	private bool ShowUserSettings { get; set; }

	private string PageClass => ShowUserSettings ? "show-page-overlay show-user-settings" : string.Empty;

	private UserProfileModel UserProfile { get; set; } = new();

	private string UserName
	{
		get
		{
			var value = string.Empty;

			if (UserProfile is not null)
			{
				if (!string.IsNullOrWhiteSpace(UserProfile.FirstName) && !string.IsNullOrWhiteSpace(UserProfile.LastName))
				{
					value = $"{UserProfile.FirstName.First()}{UserProfile.LastName.First()}".Trim();
				}
				else if (!string.IsNullOrWhiteSpace(UserProfile.FirstName) && string.IsNullOrWhiteSpace(UserProfile.LastName))
				{
					value = $"{UserProfile.FirstName.First()}".Trim();
				}
				else if (!string.IsNullOrWhiteSpace(UserProfile.LastName))
				{
					value = $"{UserProfile.LastName.First()}".Trim();
				}
			}

			return value;
		}
	}

	private bool ShowLoader { get; set; }

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await LoadCurrentUserProfile();
		await base.OnInitializedAsync();
	}

	private async Task LoadCurrentUserProfile(UserProfileModel? model = null)
	{
		ShowPageLoader();

		if (model is not null)
		{
			UserProfile = model;
		}
		else
		{
			var result = await UserProfileService.GetCurrentUserProfile();
			UserProfile = result?.Content ?? new();
		}

		HidePageLoader();
	}

	private void ToggleUserSettings()
	{
		ShowUserSettings = !ShowUserSettings;
	}

	private void CloseUserSettings()
	{
		ShowUserSettings = false;
	}

	#endregion //Private Methods
}