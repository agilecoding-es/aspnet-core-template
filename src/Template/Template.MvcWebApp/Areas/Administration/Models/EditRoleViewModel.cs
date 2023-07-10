using System.ComponentModel.DataAnnotations;

namespace Template.MvcWebApp.Areas.Administration.Models
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
