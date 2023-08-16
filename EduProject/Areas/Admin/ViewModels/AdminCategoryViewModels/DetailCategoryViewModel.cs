using Org.BouncyCastle.Asn1.Mozilla;

namespace EduProject.Areas.Admin.ViewModels.AdminCategoryViewModels
{
    public class DetailCategoryViewModel
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<string> Courses { get; set; }
    }
}
