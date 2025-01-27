using ChangeTracking;
using Microsoft.AspNetCore.Components;
using ToDoApp.Client.Services;
using ToDoApp.Client.Shared;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Validations;

namespace ToDoApp.Client.Pages;

[Route($"{PageRoute.Profile}")]
public partial class Profile
{
	#region Fields

	private FluentValidationValidator? _fluentValidationValidator;
	private UserProfileModel _model;

	#endregion Fields

	#region Private Properties

	[Inject] private IUserProfileService UserProfileService { get; set; }

	private UserProfileModel Model
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

	private bool IsReadyOnly { get; set; } = true;

	private bool HasModelChanged 
	{
		get
		{
			return Model.CastToIChangeTrackable().IsChanged;
		}
	}

	#endregion Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		MainLayout!.ShowPageLoader();
		var result = await UserProfileService.GetCurrentUserProfile();
		Model = result?.Content ?? new();
		MainLayout!.HidePageLoader();
		await base.OnInitializedAsync();
	}

	private async Task Save()
	{
		var isValid = await _fluentValidationValidator!.ValidateAsync();

		MainLayout!.ShowPageLoader();

		if (isValid && HasModelChanged)
		{
			var result = await UserProfileService.Save(Model);
			if (result.IsSuccessStatusCode && result.Content is not null)
			{
				Model = result.Content;
				IsReadyOnly = true;
				await MainLayout!.RefreshUserSettings(Model);
			}
		}

		MainLayout!.HidePageLoader();
	}

	private void Edit()
	{
		IsReadyOnly = false;
	}

	private async Task Cancel()
	{
		var trackable = Model.CastToIChangeTrackable();
		if (HasModelChanged)
		{
			trackable.RejectChanges();
			//await _fluentValidationValidator!.ValidateAsync();
			await Task.Delay(0);
		}
		IsReadyOnly = true;
	}

	#endregion Private Methods
}