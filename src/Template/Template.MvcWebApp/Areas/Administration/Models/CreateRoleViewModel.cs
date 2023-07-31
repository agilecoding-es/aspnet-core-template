using System.ComponentModel.DataAnnotations;

namespace Template.MvcWebApp.Areas.Administration.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
