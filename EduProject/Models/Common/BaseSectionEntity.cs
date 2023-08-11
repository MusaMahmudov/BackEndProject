namespace EduProject.Models.Common
{
    public abstract class BaseSectionEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set;}
    }
}
