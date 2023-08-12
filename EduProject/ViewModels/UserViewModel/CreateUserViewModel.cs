using System.ComponentModel.DataAnnotations;

namespace EduProject.ViewModels.UserViewModel
{
    public class CreateUserViewModel
    {
        [Required,MaxLength(256)]
        public string userName { get; set; }
        [Required, MaxLength(256)]
        public string fullName { get; set; }
        [Required, MaxLength(256),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,MaxLength(256),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(256), DataType(DataType.Password),Compare(nameof(Password))]
        public string confirmPassword { get; set; }
    }
}
