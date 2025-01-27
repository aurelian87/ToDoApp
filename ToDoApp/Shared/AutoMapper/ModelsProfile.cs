using AutoMapper;
using ToDoApp.Shared.Models;

namespace ToDoApp.Shared.AutoMapper
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<TodoModel, TodoModel>().ReverseMap();
        }
    }
}