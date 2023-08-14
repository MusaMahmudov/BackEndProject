using AutoMapper;
using EduProject.Models;
using EduProject.ViewModels.BlogViewModels;

namespace EduProject.Mappers
{
    public class BlogMapperProfile : Profile
    {
        public BlogMapperProfile() 
        {
        CreateMap<Blog,BlogViewModel>().ReverseMap();
        }
    }
}
