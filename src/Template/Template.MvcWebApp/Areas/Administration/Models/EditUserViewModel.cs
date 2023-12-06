using System.ComponentModel.DataAnnotations;
using Template.Common;

namespace Template.MvcWebApp.Areas.Administration.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression(pattern: RegExPatterns.Validators.Username, ErrorMessage = "The {0} field is not a valid username.")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(pattern: RegExPatterns.Validators.Email, ErrorMessage = "The {0} field is not a valid e-mail address.")]
        public string Email { get; set; }
        public bool IsLockout { get; internal set; }

        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
