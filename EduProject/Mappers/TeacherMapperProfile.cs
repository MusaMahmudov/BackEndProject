using AutoMapper;
using EduProject.Models;
using EduProject.ViewModels.TeacherViewModels;

namespace EduProject.Mappers
{
	public class TeacherMapperProfile : Profile
	{
	public TeacherMapperProfile() 
		{
			CreateMap<Teacher, TeacherViewModel>().ReverseMap();
		
		}

	}
}
