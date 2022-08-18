using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Shared.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}