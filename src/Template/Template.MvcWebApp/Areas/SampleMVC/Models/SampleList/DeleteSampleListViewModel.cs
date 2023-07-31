using System.ComponentModel.DataAnnotations;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.SampleMvc.Models.SampleList
{
    public class DeleteSampleListViewModel 
    {
        public SampleListViewModel SampleList { get; set; }

        public bool NeedsConfirmation { get; set; }

        public bool Confirmed { get; set; }

    }
}
