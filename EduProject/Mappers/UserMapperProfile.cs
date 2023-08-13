using AutoMapper;
using EduProject.Areas.Admin.ViewModels.UserViewModels;
using EduProject.Models.Identity;
using EduProject.ViewModels.UserViewModel;

namespace EduProject.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<CreateUserViewModel, AppUser>().ReverseMap();

            CreateMap<AppUser,UserViewModel>().ReverseMap();
            CreateMap<AppUser,ChangeUserViewModel>().ReverseMap();



        }
    }
}
