using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Template.Application.Identity;
using Template.Application.Sample.Queries;
using Template.Domain.Entities.Sample;
using Template.MvcWebApp.Areas.Sample.Models.SampleList;

namespace Template.MvcWebApp.Areas.Sample.Controllers
{
    public class SampleListController : BaseSampleController
    {
        private readonly IMediator _mediator;
        private readonly UserManager _userManager;

        public SampleListController(IMediator mediator, UserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public ClaimsPrincipal User => HttpContext?.User!;

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var result = await _mediator.Send(new GetSampleListByUser.Query(user));

            return View(result.Value.Adapt<List<SampleListViewModel>>());
        }


        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User);

            var result = await _mediator.Send(new GetSampleListByUser.Query(user));

            return Json(result.Value.Adapt<List<SampleListViewModel>>());
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetSampleListById.Query(new SampleListKey(id)));

            if (result.IsFailure)
            {

            }

            return View(result.Value);

        }
    }
}
