using System.ComponentModel.DataAnnotations;

namespace EduProject.ViewModels.LoginViewModels;

public class LoginViewModel
{
    [Required,MaxLength(256)]
    public string MailOrUsername { get; set; }
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }

}
