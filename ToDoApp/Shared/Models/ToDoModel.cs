namespace ToDoApp.Shared.Models
{
    public class ToDoModel
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime DueDate { get; set; }
    }
}