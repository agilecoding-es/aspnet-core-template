using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.Sample.Models.SampleList
{
    public class SampleListWithItemCountsViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Items count")]
        public int ItemsCount { get; set; }

    }
}
