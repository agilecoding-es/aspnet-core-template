using System.ComponentModel.DataAnnotations;

namespace Template.MvcWebApp.Areas.Sample.Models.SampleList
{
    public class SampleListViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Items count")]
        public int ItemsCount { get; set; }
    }
}
