﻿using MediatR;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Features.IdentityContext;

namespace Template.WebApp.Areas.SampleAjax.Controllers
{
    public class SampleItemController : BaseSampleController
    {

        public SampleItemController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
        }

    }
}
