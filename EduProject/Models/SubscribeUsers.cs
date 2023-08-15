using EduProject.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduProject.Models
{
    public class SubscribeUsers : BaseEntity
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
