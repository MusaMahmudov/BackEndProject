using System.ComponentModel.DataAnnotations;

namespace EduProject.ViewModels.LoginViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
