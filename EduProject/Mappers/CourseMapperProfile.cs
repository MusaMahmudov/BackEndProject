using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Models;

namespace EduProject.Mappers
{
    public class CourseMapperProfile : Profile
    {
        public CourseMapperProfile() 
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<CreateCourseViewModel, Course>().ReverseMap();
            CreateMap<Course, DetailCourseViewModel>()
                .ForMember(cvc=>cvc.Category,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.Category)))
                .ReverseMap();
            CreateMap<Course, UpdateCourseViewModel>().ForMember(c=>c.Image,c=>c.Ignore())
                .ForMember(cvc=>cvc.CategoryId,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.CategoryId))).ReverseMap();


        }
    }
}
