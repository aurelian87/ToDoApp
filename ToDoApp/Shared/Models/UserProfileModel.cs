namespace ToDoApp.Shared.Models;

public class UserProfileModel
{
	public virtual int Id { get; set; }

	public virtual string? FirstName { get; set; }

	public virtual string? LastName { get; set; }

	public virtual string? Email { get; set; }

	public virtual string? PhoneNumber { get; set; }

	public virtual string? Address { get; set; }

	public virtual string? City { get; set; }

	public virtual string? Country { get; set; }

	public virtual string? PostalCode { get; set; }

	public virtual string? Image { get; set; }
}