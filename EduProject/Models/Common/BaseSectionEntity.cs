namespace EduProject.Models.Common
{
    public abstract class BaseSectionEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set;}
    }
}
