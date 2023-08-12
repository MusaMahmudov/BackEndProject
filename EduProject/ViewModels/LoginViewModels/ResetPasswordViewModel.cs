using System.ComponentModel.DataAnnotations;

namespace EduProject.ViewModels.LoginViewModels
{
    public class ResetPasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
