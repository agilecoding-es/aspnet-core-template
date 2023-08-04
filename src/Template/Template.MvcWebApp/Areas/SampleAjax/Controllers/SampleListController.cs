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
            var result = await GetSamplelistAsync(id, cancellationToken);

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

                var sampleListKey = createListResult.Value;
                return RedirectToAction(nameof(EditFromSuccessRedirection), new { id = sampleListKey });
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var result = await GetSamplelistAsync(id, "Edit", cancellationToken);

            var model = result.Value.Adapt<EditSampleListViewModel>();

            return View(model);
        }

        public async Task<IActionResult> EditFromSuccessRedirection(int id, CancellationToken cancellationToken)
        {
            var result = await GetSamplelistAsync(id, "Edit", cancellationToken);

            var model = new EditSampleListViewModel();
            if (result.IsSuccess)
            {
                model = result.Value.Adapt<EditSampleListViewModel>();
                //TODO: CORREGIR
                //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Create_Success"].Value);
            }

            return View("Edit", model);
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

                var getResult = await GetSamplelistAsync(sampleListViewModel.Id, "Edit", cancellationToken);
                var model = getResult.Value.Adapt<EditSampleListViewModel>();
                if (result.IsFailure)
                {
                    HandleFailureResult(result);
                }
                else
                {
                    sampleListViewModel = model;
                    //TODO: CORREGIR
                    //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value)
                    //                                                  .SetId("Edit");
                }
            }

            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await GetSamplelistAsync(id, cancellationToken);

            var model = new DeleteSampleListViewModel();
            if (result.IsSuccess)
            {
                model = new DeleteSampleListViewModel()
                {
                    SampleList = result.Value.Adapt<SampleListViewModel>()
                };
            }

            return PartialView("_Delete",model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSampleListViewModel deleteSampleListViewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(
                    new DeleteSampleList.Command(deleteSampleListViewModel.SampleList.Id,
                                                        DeleteWithItems: deleteSampleListViewModel.NeedsConfirmation && deleteSampleListViewModel.Confirmed),
                    cancellationToken);

                if (result.IsFailure && result.Exception is ValidationException)
                {
                    //TODO: Traducir mensaje
                    //TODO: CORREGIR
                    //var responseError = HandleErrorResult(
                    //                result,
                    //                ResponseMessageViewModel.Error(result.Exception.Message, "Do you want to delete anyway?"));

                    deleteSampleListViewModel.NeedsConfirmation = true;
                    //deleteSampleListViewModel.Error = responseError;

                    return Json(deleteSampleListViewModel);
                }

                //TODO: Cambiar mensaje
                //TODO: CORREGIR
                //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value);
            }
            Url.Action(nameof(Index));
            return  RedirectToAction(nameof(Index));
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
                    //TODO: CORREGIR
                    //var responseError = HandleErrorResult(addItemResult, id: "Items");
                    //return Json(responseError );
                }
                else
                {
                    //TODO: CORREGIR
                    //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_AddItem_Success"].Value)
                    //                                                  .SetId("Items");
                }
            }
            else
            {
                var result = await GetSamplelistAsync(sampleItemViewModel.ListId, "Edit", cancellationToken);
                var model = result.Value.Adapt<EditSampleListViewModel>();

                return View("Edit", model);
            }
            
            var partialViewContent = await razorViewRenderer.RenderPartialViewAsync("_Item", sampleItemViewModel, ControllerContext);
            
            // Devolver el contenido del partial view como JSON
            return Json(new { content = partialViewContent });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(SampleItemViewModel sampleItemViewModel, CancellationToken cancellationToken)
        {
            var removeResult = await mediator.Send(
                new RemoveSampleItemFromList.Command(
                    sampleItemViewModel.ListId,
                    sampleItemViewModel.Id), cancellationToken: cancellationToken);


            if (removeResult.IsFailure)
            {
                //TODO: CORREGIR
                //var responseError = HandleErrorResult(removeResult, id: "Items");
                //return Json(responseError);
            }
            else
            {
                //TODO: CORREGIR
                //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_RemoveItem_Success"].Value)
                //                                                  .SetId("Items");
            }

            // Devolver el contenido del partial view como JSON
            return Json(new { success = removeResult.IsSuccess });
        }

        private async Task<Result<List<SampleListWithItemsCountDto>>> ListByLoggedUserAsync(CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(UserPrincipal);

            return await mediator.Send(new ListSampleListByUser.Query(user), cancellationToken);
        }

        private async Task<Result<SampleListWithItemsDto>> GetSamplelistAsync(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSampleListById.Query(id), cancellationToken);

            if (result.IsFailure)
            {
                HandleFailureResult(result);
            }

            return result;
        }

        private async Task<Result<SampleListWithItemsDto>> GetSamplelistAsync(int id, string elementId = null, CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetSampleListById.Query(id), cancellationToken);

            if (result.IsFailure)
            {
                HandleFailureResult(result);
            }

            return result;
        }
    }
}
