using AutoMapper;
using EduProject.Areas.Admin.ViewModels.SettingsViewModels;

using EduProject.Models;

namespace EduProject.Mappers
{
    public class SettingMapperProfile : Profile
    {
        public SettingMapperProfile() 
        {
         CreateMap<Setting,UpdateSettingViewModel>().ReverseMap();
            CreateMap<CreateSettingViewModel, Setting>().ReverseMap();
            CreateMap<Setting, DeleteSettingViewModel>().ReverseMap();


        }
    }
}
