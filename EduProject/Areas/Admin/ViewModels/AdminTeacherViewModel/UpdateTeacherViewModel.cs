﻿using EduProject.Models.Common;

namespace EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel
{
    public class UpdateTeacherViewModel 
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public IFormFile? Image { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public List<string>? SkillName { get; set; }
        public List<int>? SkillId { get; set; }
        public List<byte> Percent { get; set; }
    }
}
