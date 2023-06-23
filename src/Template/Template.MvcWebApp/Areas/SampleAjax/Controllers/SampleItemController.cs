using MediatR;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Identity;

namespace Template.MvcWebApp.Areas.SampleAjax.Controllers
{
    public class SampleItemController : BaseSampleController
    {

        public SampleItemController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
        }

    }
}
