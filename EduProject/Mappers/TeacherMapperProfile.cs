using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel;
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
                .ForMember(tvc => tvc.Percent, x => x.MapFrom(t => t.TeacherSkill.Select(tp => tp.Percent)))
                .ReverseMap();
			CreateMap<Teacher,AdminTeacherViewModel>().ReverseMap();
            CreateMap<CreateTeacherViewModel, Teacher>().ReverseMap();




        }

    }
}
