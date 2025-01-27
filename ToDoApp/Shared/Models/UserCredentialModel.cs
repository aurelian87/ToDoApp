using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Shared.Models;

public class UserCredentialModel
{
	public int Id { get; set; }

	[Required(ErrorMessage = "UserName is required")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public string Password { get; set; }

	public int UserProfileId { get; set; }

	public UserProfileModel UserProfile { get; set; }
}