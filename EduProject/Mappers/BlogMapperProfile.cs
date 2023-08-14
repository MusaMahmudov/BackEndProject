using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminBlogViewModels;
using EduProject.Models;
using EduProject.ViewModels.BlogViewModels;

namespace EduProject.Mappers
{
    public class BlogMapperProfile : Profile
    {
        public BlogMapperProfile() 
        {
            CreateMap<Blog,BlogViewModel>().ReverseMap();
            CreateMap<Blog, DetailBlogViewModel>().ReverseMap();
            CreateMap<Blog, AdminDetailBlogViewModel>().ReverseMap();
            CreateMap<Blog, UpdateBlogViewModel>().ForMember(b=>b.Image,x=>x.Ignore())
                .ReverseMap();
            CreateMap<Blog, AdminCreateBlogViewModel>().ReverseMap();

        }
    }
}
