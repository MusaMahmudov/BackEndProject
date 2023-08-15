using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminSpeakerViewModels;
using EduProject.Models;
using EduProject.ViewModels.SpeakerViewModels;

namespace EduProject.Mappers
{
    public class SpeakerMapperProfile : Profile
    {
        public SpeakerMapperProfile() 
        {
          CreateMap<Speaker,SpeakerViewModel>().ReverseMap();
            CreateMap<Speaker, DetailSpeakerViewModel>().ForMember(evc=>evc.EventName,x=>x.MapFrom(s=>s.eventSpeakers.Select(evc=>evc.Event.Name))).ReverseMap();
            CreateMap<CreateSpeakerViewModel, Speaker>().ReverseMap();
            CreateMap<Speaker, UpdateSpeakerViewModel>()
                .ForMember(svc=>svc.eventId,x=>x.MapFrom(s=>s.eventSpeakers.Select(svc=>svc.EventId)))
                .ForMember(s=>s.Image,x=>x.Ignore())
                .ReverseMap();


        }
    }
}
