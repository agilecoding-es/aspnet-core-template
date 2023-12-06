using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Features.Identity;
using Template.MvcWebApp.Controllers;

namespace Template.MvcWebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize()]
    public class BaseAdministrationController : BaseController
    {
        protected readonly IMediator mediator;
        protected readonly UserManager userManager;
        protected readonly RoleManager roleManager;

        public BaseAdministrationController(IMediator mediator, UserManager userManager, RoleManager roleManager, IHtmlLocalizer localizer) : base(localizer)
        {
            this.mediator = mediator;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
    }
}
