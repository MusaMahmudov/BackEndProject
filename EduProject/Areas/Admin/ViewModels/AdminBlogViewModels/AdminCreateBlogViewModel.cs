namespace EduProject.Areas.Admin.ViewModels.AdminBlogViewModels
{
    public class AdminCreateBlogViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }

        public string Description { get; set; }
    }
}
