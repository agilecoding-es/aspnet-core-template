using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.Sample.Models.SampleList
{
    public class EditSampleListViewModel
    {
        public EditSampleListViewModel() { }

        public EditSampleListViewModel(Guid id)
        {
            Id = id;
            NewItem = new SampleItemViewModel()
            {
                ListId = id
            };
        }

        public Guid Id { get; set; }

        [Display(Name = "List Name")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Items Count")]
        public int ItemsCount => Items.Count;

        public List<SampleItemViewModel> Items { get; set; } = new List<SampleItemViewModel>();
        public SampleItemViewModel NewItem { get; set; }

    }
}
