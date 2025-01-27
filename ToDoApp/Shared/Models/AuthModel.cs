using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Shared.Models;

public class AuthModel
{
	[Required(ErrorMessage = "UserName is required")]
	public virtual string UserName { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public virtual string Password { get; set; }
}