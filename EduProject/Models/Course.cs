﻿using EduProject.Models.Common;

namespace EduProject.Models
{
    public class Course : BaseSectionEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Start { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int Students { get; set; }
        public string Assestment { get; set; }
        public ICollection<CourseCategory> courseCategories { get; set; }



    }
}
