using ChangeTracking;
using Microsoft.AspNetCore.Components;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Validations;

namespace ToDoApp.Client.Pages;

[Route($"{PageRoute.TodoDetails}")]
public partial class TodoDetails
{
	#region Fields

	private FluentValidationValidator? _fluentValidationValidator;
	private TodoModel? _model;

	#endregion //Fields

	#region Parameters

	[Parameter] public int ToDoId { get; set; }

	#endregion //Parameters

	#region Private Properties

	[Inject] private ITodoService? ToDoService { get; set; }

	private TodoModel Model
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
				_model = _model?.AsTrackable();
			}
		}
	}

	private bool ShowCancelPopup { get; set; }

	#endregion //Private Properties 

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		if (ToDoId > 0)
		{
			var todo = await ToDoService!.GetById(ToDoId);
			Model = todo;
		}
		else
		{
			Model = new TodoModel
			{
				DueDate = DateTime.Now
			};
		}

		await base.OnInitializedAsync();
	}

	private async Task Save()
	{
		var isValid = await _fluentValidationValidator!.ValidateAsync();
		if (isValid)
		{
			await ToDoService!.Save(Model);
		}
		NavigationManager?.NavigateTo(PageRoute.Home);
	}

	private void Cancel()
	{
		var trackable = Model.CastToIChangeTrackable();

		if (trackable.IsChanged)
		{
			ShowCancelPopup = true;
		}
		else
		{
			NavigationManager?.NavigateTo(PageRoute.Home);
		}
	}

	private void Reset()
	{
		NavigationManager?.NavigateTo(PageRoute.Home);
	}

	#endregion //Private Methods
}