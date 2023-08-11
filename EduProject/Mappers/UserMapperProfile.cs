using AutoMapper;
using EduProject.Models.Identity;
using EduProject.ViewModels.UserViewModel;

namespace EduProject.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
         CreateMap<CreateUserViewModel,AppUser>().ReverseMap();

        }
    }
}
