using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Routing;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Identity;
using Template.Application.Sample.Commands;
using Template.Application.Sample.Queries;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.MvcWebApp.Areas.Sample.Models.SampleList;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Areas.Sample.Controllers
{
    public class SampleItemController : BaseSampleController
    {

        public SampleItemController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
        }

    }
}
