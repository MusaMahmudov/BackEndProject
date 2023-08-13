using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminSliderViewModels;
using EduProject.Models;
using EduProject.ViewModels.SliderViewModels;

namespace EduProject.Mappers
{
    public class SliderMapperProfile : Profile
    {
        public SliderMapperProfile() 
        {
         CreateMap<Slider,SliderViewModel>().ReverseMap();
            CreateMap<Slider,DetailSliderViewModel>().ReverseMap();
            CreateMap<CreateSliderViewModel,Slider>().ForMember(s=>s.Image,x=>x.Ignore()).ReverseMap();
        }
    }
}
