namespace ToDoApp.Shared.Models;

public class TodoModel
{
    public virtual int Id { get; set; }

    public virtual string Title { get; set; }

    public virtual string Description { get; set; }

    public virtual DateTime DueDate { get; set; }

	public virtual int UserProfileId { get; set; }
}