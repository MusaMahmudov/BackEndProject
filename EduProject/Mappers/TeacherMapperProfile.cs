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
			CreateMap<Teacher,	DetailTeacherViewModel>()
				.ForMember(tvc => tvc.SkillNames,x=> x.MapFrom(t=> t.TeacherSkill.Select(tp=>tp.Skill.Name)))
				.ReverseMap();

			
				
		
		}

	}
}
