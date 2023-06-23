using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Identity;
using Template.MvcWebApp.Controllers;

namespace Template.MvcWebApp.Areas.SampleAjax.Controllers
{
    [Area("SampleAjax")]
    [Authorize()]
    public class BaseSampleController : BaseController
    {
        protected readonly IMediator mediator;
        protected readonly UserManager userManager;

        public BaseSampleController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(localizer)
        {
            this.mediator = mediator;
            this.userManager = userManager;
        }
    }
}
