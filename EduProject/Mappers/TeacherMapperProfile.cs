﻿using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel;
using EduProject.Models;
using EduProject.ViewModels.TeacherViewModels;

namespace EduProject.Mappers
{
	public class TeacherMapperProfile : Profile
	{
	public TeacherMapperProfile() 
		{
			CreateMap<Teacher, TeacherViewModel>().ForMember(tvc => tvc.socialMedia, x => x.MapFrom(t => t.socialMedia)).ReverseMap();
			CreateMap<Teacher,	DetailTeacherViewModel>()
				.ForMember(tvc => tvc.SkillNames,x=> x.MapFrom(t=> t.TeacherSkill.Select(tp=>tp.Skill.Name)))
                .ForMember(tvc => tvc.Percent, x => x.MapFrom(t => t.TeacherSkill.Select(tp => tp.Percent)))
                .ReverseMap();
			CreateMap<Teacher,AdminTeacherViewModel>().ReverseMap();
            CreateMap<Teacher, DeleteTeacherViewModel>().ReverseMap();
            CreateMap<UpdateTeacherViewModel, TeacherSkill>().ForMember(ts=>ts.Percent,x=>x.MapFrom(ut=>ut.Percent))
				.ReverseMap();

            CreateMap<Teacher, UpdateTeacherViewModel>().ForMember(ut=>ut.SkillName,x=>x.MapFrom(t=>t.TeacherSkill.Select(t=>t.Skill.Name)))
                  .ForMember(tvc => tvc.Percent, x => x.MapFrom(t => t.TeacherSkill.Select(tp => tp.Percent)))
                .ForMember(ut=>ut.Image,x=>x.Ignore())
				.ReverseMap();


            CreateMap<Teacher, AdminDetailTeacherViewModel>().ForMember(dt=>dt.Percent,x=>x.MapFrom(t=>t.TeacherSkill.Select(t=>t.Percent)))
				.ForMember(dt=>dt.SkillName,x=>x.MapFrom(t=>t.TeacherSkill.Select(t=>t.Skill.Name)))
				.ReverseMap();

            CreateMap<CreateTeacherViewModel, Teacher>()
				.ReverseMap();




        }

    }
}
