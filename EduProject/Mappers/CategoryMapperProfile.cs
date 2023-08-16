using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCategoryViewModels;
using EduProject.Models;
using EduProject.ViewModels.CategoryViewModels;

namespace EduProject.Mappers
{
    public class CategoryMapperProfile :Profile
    {
        public CategoryMapperProfile() 
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();

            CreateMap<CreateCategoryViewModel,Category>().ReverseMap();
            CreateMap<Category, DetailCategoryViewModel>().ForMember(cvc=>cvc.Courses,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.Course.Name))).ReverseMap();
            CreateMap<Category, UpdateCategoryViewModel>().ForMember(cvc=>cvc.CourseId,x=>x.MapFrom(c=>c.courseCategories.Select(cvc=>cvc.CourseId)))
                .ReverseMap();

        }
    }
}
