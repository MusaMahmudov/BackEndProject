using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Models;
using EduProject.ViewModels.CourseViewModels;

namespace EduProject.Mappers
{
    public class CourseMapperProfile : Profile
    {
        public CourseMapperProfile() 
        {
			CreateMap<Course, HomeCourseViewModel>().ReverseMap();
            

			CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<Course, CoursePageViewModel>().ReverseMap();
            CreateMap<Course, PageDetailCourseViewModel>().ForMember(cvc=>cvc.Category,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.Category)))
                .ReverseMap();

            CreateMap<CourseCategory, FilterCourseViewModel>().ForMember(fvm=>fvm.Id,x=>x.MapFrom(cc=>cc.CourseId))
                .ForMember(fvm => fvm.Image, x => x.MapFrom(cc => cc.Course.Image))
                .ForMember(fvm => fvm.Name, x => x.MapFrom(cc => cc.Course.Name))
                .ForMember(fvm => fvm.Description, x => x.MapFrom(cc => cc.Course.Description))
                .ReverseMap();

            CreateMap<CreateCourseViewModel, Course>().ReverseMap();
            CreateMap<Course, DetailCourseViewModel>()
                .ForMember(cvc=>cvc.Category,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.Category)))
                .ReverseMap();
            CreateMap<Course, UpdateCourseViewModel>().ForMember(c=>c.Image,c=>c.Ignore())
                .ForMember(cvc=>cvc.CategoryId,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.CategoryId))).ReverseMap();


        }
    }
}
