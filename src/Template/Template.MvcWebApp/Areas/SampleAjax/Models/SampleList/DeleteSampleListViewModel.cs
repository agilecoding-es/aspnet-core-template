using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.SampleAjax.Models.SampleList
{
    public class DeleteSampleListViewModel 
    {
        public int ListId { get; set; }
        
        [Display(Name = "Name")]
        public string ListName { get; set; }

        [Display(Name = "Items Count")]
        public int ItemsCount { get; internal set; }
        
        public bool HasItems { get; set; }
    }
}
