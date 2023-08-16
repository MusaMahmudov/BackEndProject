﻿using AutoMapper;
using EduProject.Areas.Admin.ViewModels.SkillViewModels;
using EduProject.Models;

namespace EduProject.Mappers
{
    public class SkillMapperProfile : Profile
    {
        public SkillMapperProfile() 
        {
         CreateMap<Skill,SkillViewModel>().ReverseMap();
            CreateMap<CreateSkillViewModel, Skill>().ReverseMap();
            CreateMap<Skill, DetailSkillViewModel>().ForMember(ds=>ds.TeacherName,x=>x.MapFrom(s=>s.TeacherSkill.Select(ts=>ts.Teacher.Name))).ReverseMap();
            CreateMap<Skill, DeleteSkillViewModel>().ReverseMap();
            CreateMap<Skill, UpdateSkillViewModel>().ReverseMap();



        }
    }
}
