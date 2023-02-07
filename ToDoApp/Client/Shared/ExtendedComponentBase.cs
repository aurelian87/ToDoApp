using Microsoft.AspNetCore.Components;

namespace ToDoApp.Client.Shared;

public abstract class ExtendedComponentBase : ComponentBase
{
    #region Private Properties

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected void ShowConsoleMessage(string message)
    {
        Console.WriteLine(message);
    }

    #endregion //Private Methods
}