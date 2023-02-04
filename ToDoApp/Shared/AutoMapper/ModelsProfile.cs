using AutoMapper;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.ModelsDto;

namespace ToDoApp.Shared.AutoMapper
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<ToDoModelDto, ToDoModel>().ReverseMap();
            CreateMap<ToDoModel, ToDoModel>().ReverseMap();
        }
    }
}