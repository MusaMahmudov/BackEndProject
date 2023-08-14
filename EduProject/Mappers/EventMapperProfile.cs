using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminEventViewModels;
using EduProject.Models;
using EduProject.ViewModels.EventViewModels;

namespace EduProject.Mappers
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile() 
        {
          CreateMap<Event,EventViewModel>().ReverseMap();
            CreateMap<Event, DetailEventViewModel>()
                .ForMember(evc=>evc.Speakers,x=>x.MapFrom(e=>e.eventSpeakers.Select(es=>es.Speaker)))
                .ReverseMap();
            CreateMap<Event, AdminEventViewModel>().ReverseMap();
            CreateMap<CreateEventViewModel,Event>() .ReverseMap();
            CreateMap<Event,AdminDetailEventViewModel>().ForMember(evc=>evc.Speakers,x=>x.MapFrom(e=>e.eventSpeakers.Select(s=>s.Speaker.Name))).ReverseMap();

        }
    }
}
