using EduProject.Models.Common;

namespace EduProject.ViewModels.TeacherViewModels
{
	public class TeacherViewModel : BaseEntity
	{
		public string Name { get; set; }
		public string Rank { get; set; }
		public string Image { get; set; }
	}
}
