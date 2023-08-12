using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduProject.Models.Identity
{
    public class AppUser : IdentityUser
    {
        [Required,MaxLength(256)]
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
    }
}
