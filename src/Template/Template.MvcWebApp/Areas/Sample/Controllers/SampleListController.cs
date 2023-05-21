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
using Microsoft.AspNetCore.Mvc.Routing;
using Result = Template.Domain.Entities.Shared.Result;

namespace Template.MvcWebApp.Areas.Sample.Controllers
{
    public class SampleListController : BaseSampleController
    {
        public SampleListController(IMediator mediator, UserManager userManager, IHtmlLocalizer localizer) : base(mediator, userManager, localizer)
        {
        }

        public ClaimsPrincipal UserPrincipal => HttpContext?.User!;

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await ListByLoggedUserAsync(cancellationToken);

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            return View(result.IsSuccess ? result.Value.Adapt<List<SampleListWithItemCountsViewModel>>() : new List<SampleListWithItemCountsViewModel>());
        }

        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var result = await ListByLoggedUserAsync(cancellationToken);

            return Json(result.Adapt<List<SampleListViewModel>>());
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
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
                return RedirectToAction(nameof(EditFromSuccessRedirection), new { id = sampleListKey.Value });
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var result = await GetSamplelistAsync(id, "Edit", cancellationToken);

            var model = result.Value.Adapt<EditSampleListViewModel>();
            model.NewItem = new SampleItemViewModel() { ListId = model.Id };

            return View(model);
        }

        public async Task<IActionResult> EditFromSuccessRedirection(Guid id, CancellationToken cancellationToken)
        {
            var result = await GetSamplelistAsync(id, "Edit", cancellationToken);

            var model = new EditSampleListViewModel();
            if (result.IsSuccess)
            {
                model = result.Value.Adapt<EditSampleListViewModel>();
                model.NewItem = new SampleItemViewModel() { ListId = model.Id };
                ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Create_Success"].Value);
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
                        new SampleListKey(sampleListViewModel.Id),
                        sampleListViewModel.Name,
                        sampleListViewModel.UserId), cancellationToken);

                var getResult = await GetSamplelistAsync(sampleListViewModel.Id, "Edit", cancellationToken);
                var model = getResult.Value.Adapt<EditSampleListViewModel>();
                model.NewItem = new SampleItemViewModel() { ListId = model.Id };
                if (result.IsFailure)
                {
                    sampleListViewModel.Items = model.Items;
                    HandleErrorResult(result, id: "Edit");
                }
                else
                {
                    sampleListViewModel = model;
                    ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value)
                                                                      .SetId("Edit");
                }
            }

            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSampleListViewModel deleteSampleListViewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(
                    new DeleteSampleList.Command(new SampleListKey(deleteSampleListViewModel.SampleList.Id),
                                                        DeleteWithItems: deleteSampleListViewModel.NeedsConfirmation && deleteSampleListViewModel.Confirmed),
                    cancellationToken);

                if (result.IsFailure)
                {
                    //TODO: Traducir mensaje
                    HandleErrorResult(result,
                                      ResponseMessageViewModel.Error(result.Exception.Message, "Do you want to delete anyway?"));
                    deleteSampleListViewModel.NeedsConfirmation = true;

                    return View(deleteSampleListViewModel);
                }

                //TODO: Cambiar mensaje
                ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(SampleItemViewModel sampleItemViewModel, CancellationToken cancellationToken)
        {
            Result addItemResult = null;
            if (ModelState.IsValid)
            {
                addItemResult = await mediator.Send(
                    new AddSampleItemToList.Command(
                        new SampleListKey(sampleItemViewModel.ListId),
                        sampleItemViewModel.Adapt<SampleItemDto>()), cancellationToken: cancellationToken);

                if (addItemResult.IsFailure)
                {
                    HandleErrorResult(addItemResult, id: "Items");
                }
                else
                {
                    ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value)
                                                                      .SetId("Items");
                }
            }

            var result = await GetSamplelistAsync(sampleItemViewModel.ListId, "Edit", cancellationToken);
            var model = result.Value.Adapt<EditSampleListViewModel>();
            if (result.IsSuccess)
            {
                model.NewItem = new SampleItemViewModel() { ListId = model.Id };
            }

            if (addItemResult != null && addItemResult.IsFailure)
            {
                model.NewItem = sampleItemViewModel;
            }

            return View("Edit", model);
        }

        private async Task<Result<List<SampleListWithItemsCountDto>>> ListByLoggedUserAsync(CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(UserPrincipal);

            return await mediator.Send(new ListSampleListByUser.Query(user), cancellationToken);
        }

        private async Task<Result<SampleListWithItemsDto>> GetSamplelistAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSampleListById.Query(new SampleListKey(id)), cancellationToken);

            if (result.IsFailure)
            {
                HandleErrorResult(result);
            }

            return result;
        }
        private async Task<Result<SampleListWithItemsDto>> GetSamplelistAsync(Guid id, string elementId = null, CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetSampleListById.Query(new SampleListKey(id)), cancellationToken);

            if (result.IsFailure)
            {
                HandleErrorResult(result, elementId);
            }

            return result;
        }
    }
}
