using AutoMapper;
using EduProject.Areas.Admin.ViewModels.UserViewModels;
using EduProject.Models.Identity;
using EduProject.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Identity;

namespace EduProject.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<CreateUserViewModel, AppUser>()
                .ReverseMap();
            CreateMap<AppUser, UpdateUserViewModel>().ReverseMap();
            CreateMap<AppUser, DetailUserViewModel>().ReverseMap();
            CreateMap<IdentityRole, DetailUserViewModel>().ForMember(du => du.Role,x=>x.MapFrom(ir=>ir.Name.ToList())).ReverseMap();




            CreateMap<AppUser,UserViewModel>().ReverseMap();
            CreateMap<AppUser, ChangeUserViewModel>()
                .ReverseMap();
            
            CreateMap<AppUser,StatusUserViewModel>().ReverseMap();



        }
    }
}
