using IdentityModel.OidcClient;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Security.Claims;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Exceptions;
using Template.Application.Identity;
using Template.Application.Sample.Commands;
using Template.Application.Sample.Queries;
using Template.Common.Extensions;
using Template.Domain.Entities.Shared;
using Template.MvcWebApp.Areas.SampleAjax.Models.SampleList;
using Template.MvcWebApp.Models;
using Template.MvcWebApp.Services.Rendering;
using Result = Template.Domain.Entities.Shared.Result;

namespace Template.MvcWebApp.Areas.SampleAjax.Controllers
{
    public class SampleListController : BaseSampleController
    {
        private readonly IRazorViewRenderer razorViewRenderer;

        public SampleListController(IRazorViewRenderer razorViewRenderer, IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
            this.razorViewRenderer = razorViewRenderer;
        }

        public ClaimsPrincipal UserPrincipal => HttpContext?.User!;

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await ListByLoggedUserAsync(cancellationToken);

            return View(result.IsSuccess ? result.Value.Adapt<List<SampleListWithItemCountsViewModel>>() : new List<SampleListWithItemCountsViewModel>());
        }

        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var result = await ListByLoggedUserAsync(cancellationToken);

            return Json(result.IsSuccess ? result.Value.Adapt<List<SampleListWithItemCountsViewModel>>() : new List<SampleListWithItemCountsViewModel>());
        }

        public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
        {
            var result = await GetSampleListByIdAsync(id, cancellationToken: cancellationToken);

            return View(result.Value.Adapt<SampleListViewModel>());

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SampleListViewModel sampleListViewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(UserPrincipal);

                var createListResult = await mediator.Send(new CreateSampleList.Command(user, sampleListViewModel.Name), cancellationToken);

                if (createListResult.IsFailure)
                {
                    HandleFailureResult(createListResult);
                    return View(sampleListViewModel);
                }

                if (createListResult.IsSuccess && !sampleListViewModel.Items.IsNullOrEmpty())
                {
                    var addItemsResult = await mediator.Send(new AddSampleItemsToList.Command(createListResult.Value, sampleListViewModel.Items.Adapt<List<SampleItemDto>>()));

                    if (addItemsResult.IsFailure)
                    {
                        HandleFailureResult(createListResult);
                        return View(sampleListViewModel);
                    }

                }

                var sampleListId = createListResult.Value;
                return RedirectToAction(nameof(Edit), new { id = sampleListId });
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var result = await GetSampleListByIdAsync(id, "Edit", cancellationToken);

            var model = result.Value.Adapt<EditSampleListViewModel>();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSampleListViewModel sampleListViewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(
                    new UpdateSampleList.Command(
                        sampleListViewModel.Id,
                        sampleListViewModel.Name,
                        sampleListViewModel.UserId), cancellationToken);

                if (result.IsFailure)
                {
                    return Json(GetFailureMessageResponse(result, "Edit"));
                }
                else
                {
                    return Json(GetSuccessMessageResponse(localizer["Sample_SampleList_Edit_Success"].Value, "Edit"));
                }
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await GetSampleListByIdAsync(id, cancellationToken: cancellationToken);

            var model = new DeleteSampleListViewModel();
            if (result.IsSuccess)
            {
                var sampleList = result.Value.Adapt<SampleListViewModel>();
                model = new DeleteSampleListViewModel()
                {
                    ListId = sampleList.Id,
                    ListName = sampleList.Name,
                    ItemsCount = sampleList.ItemsCount,
                    HasItems = sampleList.Items.Any()
                };
            }

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSampleListViewModel deleteSampleListViewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(
                    new DeleteSampleList.Command(deleteSampleListViewModel.ListId),
                    cancellationToken);

                if (result.IsSuccess)
                {
                    return Json(GetSuccessMessageResponse(localizer.GetString("Sample_SampleList_Delete_Success")));
                }
                else
                {
                    return Json(GetFailureMessageResponse(result));
                }
            }
            return Json(deleteSampleListViewModel);
        }

        public async Task<IActionResult> Items(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSampleItemsByListId.Query(id), cancellationToken);

            return PartialView("_ItemList", result.Value.Adapt<List<SampleItemViewModel>>());
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(SampleItemViewModel sampleItemViewModel, CancellationToken cancellationToken)
        {
            Result addItemResult = null;
            if (ModelState.IsValid)
            {
                addItemResult = await mediator.Send(
                    new AddSampleItemToList.Command(
                        sampleItemViewModel.ListId,
                        sampleItemViewModel.Adapt<SampleItemDto>()), cancellationToken: cancellationToken);

                if (addItemResult.IsFailure)
                {
                    return Json(GetFailureMessageResponse(addItemResult, "AddItemValidations"));
                }
            }

            var partialViewContent = await razorViewRenderer.RenderPartialViewAsync("_Item", sampleItemViewModel, ControllerContext);

            // Devolver el contenido del partial view como JSON
            return Json(new { success = true, content = partialViewContent });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(SampleItemViewModel sampleItemViewModel, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new RemoveSampleItemFromList.Command(
                    sampleItemViewModel.ListId,
                    sampleItemViewModel.Id), cancellationToken: cancellationToken);


            if (result.IsFailure)
            {
                return Json(GetFailureMessageResponse(result, elementId: "Items"));
            }
            else
            {
                return Json(GetSuccessMessageResponse(localizer.GetString("Sample_SampleList_RemoveItem_Success"), elementId: "Items"));
            }
        }

        private async Task<Result<List<SampleListWithItemsCountDto>>> ListByLoggedUserAsync(CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(UserPrincipal);

            return await mediator.Send(new ListSampleListByUser.Query(user), cancellationToken);
        }

        private async Task<Result<SampleListWithItemsDto>> GetSampleListByIdAsync(int id, string elementId = null, CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetSampleListById.Query(id), cancellationToken);

            if (result.IsFailure)
            {
                HandleFailureResult(result, elementId);
            }

            return result;
        }
    }
}
