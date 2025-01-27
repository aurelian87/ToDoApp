using Microsoft.AspNetCore.Components;

namespace ToDoApp.Client.Pages;

[Route($"{PageRoute.Home}")]
public partial class Index
{
	private async void ShowPageLoader()
	{
		MainLayout!.ShowPageLoader();
		MainLayout!.ShowPageLoader();
		await Task.Delay(2000);
		MainLayout!.HidePageLoader();	
	}
}