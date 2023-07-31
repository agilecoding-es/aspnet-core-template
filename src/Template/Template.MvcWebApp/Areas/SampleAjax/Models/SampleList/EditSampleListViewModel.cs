using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.SampleAjax.Models.SampleList
{
    public class EditSampleListViewModel
    {
        public EditSampleListViewModel() { }

        public int Id { get; set; }

        [Display(Name = "List Name")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string UserId { get; set; }
    }
}
