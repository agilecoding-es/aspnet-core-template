using System.Security.Claims;
using IdentityModel.OidcClient;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Identity;
using Template.Application.Sample.Commands;
using Template.Application.Sample.Queries;
using Template.Domain.Entities.Sample;
using Template.Domain.Entities.Shared;
using Template.Common.Extensions;
using Template.MvcWebApp.Areas.Sample.Models.SampleList;
using Template.MvcWebApp.Models;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Template.MvcWebApp.Areas.Sample.Controllers
{
    public class SampleListController : BaseSampleController
    {
        public SampleListController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
        }

        public ClaimsPrincipal UserPrincipal => HttpContext?.User!;

        public async Task<IActionResult> Index()
        {
            var result = await ListByLoggedUserAsync();

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            return View(result.IsSuccess ? result.Value.Adapt<List<SampleListViewModel>>() : new List<SampleListViewModel>());
        }

        public async Task<IActionResult> List()
        {
            var result = await ListByLoggedUserAsync();

            return Json(result.Adapt<List<SampleListViewModel>>());
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var result = await mediator.Send(new GetSampleListById.Query(new SampleListKey(id)));

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            return View(result.Value.Adapt<SampleListViewModel>());

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SampleListViewModel sampleListViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(UserPrincipal);

                var createListResult = await mediator.Send(new CreateSampleList.Command(user, sampleListViewModel.Name));
                
                if (createListResult.IsFailure)
                {
                    HandleErrorResult(createListResult);
                    return View(sampleListViewModel);
                }

                if (createListResult.IsSuccess && !sampleListViewModel.Items.IsNullOrEmpty())
                {
                    var addItemsResult = await mediator.Send(new AddSampleItemsToList.Command(createListResult.Value, sampleListViewModel.Items.Adapt<List<SampleItemDto>>()));

                    if (addItemsResult.IsFailure)
                    {
                        HandleErrorResult(createListResult);
                        return View(sampleListViewModel);
                    }

                }

                var sampleListKey = createListResult.Value;
                return RedirectToAction(nameof(EditFromRedirection), new { id = sampleListKey.Value, FromRedirect = true });
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await GetSamplelistToEditAsync(id);

            return View(result.Value.Adapt<SampleListViewModel>());
        }

        public async Task<IActionResult> EditFromRedirection(Guid id)
        {
            var result = await GetSamplelistToEditAsync(id);

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            var model = result.IsSuccess ? result.Value.Adapt<SampleListViewModel>() : new SampleListViewModel();
            ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Create_Success"].Value);

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SampleListViewModel sampleListViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(
                    new UpdateSampleList.Command(
                        new SampleListKey(sampleListViewModel.Id),
                        sampleListViewModel.Name,
                        sampleListViewModel.UserId,
                        sampleListViewModel.Items.Adapt<List<SampleItemDto>>()));

                if (result.IsFailure)
                {
                    HandleErrorResult(result);
                }

                ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value);
            }

            return View(sampleListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            return View();
        }

        private async Task<Result<List<SampleListWithItemsCountDto>>> ListByLoggedUserAsync()
        {
            var user = await userManager.GetUserAsync(UserPrincipal);

            return await mediator.Send(new ListSampleListByUser.Query(user));
        }

        private async Task<Result<SampleListWithItemsDto>> GetSamplelistToEditAsync(Guid id)
        {
            var result = await mediator.Send(new GetSampleListById.Query(new SampleListKey(id)));

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            return result;
        }
    }
}
