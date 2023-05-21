using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.Sample.Models.SampleList
{
    public class SampleItemViewModel 
    {
        public Guid Id { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Description { get; set; }

        public Guid ListId { get; set; }

    }
}
