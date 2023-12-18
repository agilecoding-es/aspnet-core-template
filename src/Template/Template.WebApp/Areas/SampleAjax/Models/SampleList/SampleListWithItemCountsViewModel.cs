using System.ComponentModel.DataAnnotations;
using Template.WebApp.Models;

namespace Template.WebApp.Areas.SampleAjax.Models.SampleList
{
    public class SampleListWithItemCountsViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Items count")]
        public int ItemsCount { get; set; }

    }
}
