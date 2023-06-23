using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.SampleAjax.Models.SampleList
{
    public class SampleListViewModel 
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Items Count")]
        public int ItemsCount => Items.Count ;

        public List<SampleItemViewModel> Items { get; set; } = new List<SampleItemViewModel>();

    }
}
