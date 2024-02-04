using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Features.IdentityContext.Services;
using Template.WebApp.Controllers;

namespace Template.WebApp.Areas.SampleAjax.Controllers
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
