﻿namespace ToDoApp.Shared.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}