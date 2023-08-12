using AutoMapper;
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

        }
    }
}
