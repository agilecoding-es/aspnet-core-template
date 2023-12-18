using System.ComponentModel.DataAnnotations;

namespace Template.WebApp.Areas.Administration.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
