using Microsoft.AspNetCore.Components;

namespace ToDoApp.Client.Shared;

public partial class PageLoader
{
	[Parameter] public bool IsVisible { get; set; }
}