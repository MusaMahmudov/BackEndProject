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
        }
    }
}
