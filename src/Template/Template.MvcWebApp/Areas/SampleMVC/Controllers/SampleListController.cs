﻿using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Security.Claims;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Identity;
using Template.Application.Sample.Commands;
using Template.Application.Sample.Queries;
using Template.Domain.Entities.Shared;
using Template.MvcWebApp.Areas.SampleMvc.Models.SampleList;
using Template.MvcWebApp.Models;
using Result = Template.Domain.Entities.Shared.Result;

namespace Template.MvcWebApp.Areas.SampleMvc.Controllers
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

            if (result.IsSuccess)
            {
                return View(result.Value.Adapt<SampleListViewModel>());
            }
            else
            {
                return NotFound();
            }
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

                if (createListResult.IsSuccess)
                {
                    var addItemsResult = await mediator.Send(new AddSampleItemsToList.Command(createListResult.Value, sampleListViewModel.Items.Adapt<List<SampleItemDto>>()));

                    if (addItemsResult.IsFailure)
                    {
                        HandleFailureResult(createListResult);

                        return View(sampleListViewModel);
                    }

                    var sampleListId = createListResult.Value;
                    AddSuccessMessage(localizer["Sample_SampleList_Edit_Success"].Value, "Edit");
                    return RedirectToAction(nameof(Edit), new { id = sampleListId });
                }
            }
            return View(sampleListViewModel);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var result = await GetSampleListByIdAsync(id, cancellationToken: cancellationToken);

            if (result.IsSuccess)
            {
                var model = result.Value.Adapt<EditSampleListViewModel>();
                model.NewItem = new SampleItemViewModel() { ListId = model.Id };

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //public async Task<IActionResult> EditFromSuccessRedirection(int id, CancellationToken cancellationToken)
        //{
        //    var result = await GetSampleListByIdAsync(id, "Edit", cancellationToken);

        //    var model = new EditSampleListViewModel();
        //    if (result.IsSuccess)
        //    {
        //        model = result.Value.Adapt<EditSampleListViewModel>();
        //        model.NewItem = new SampleItemViewModel() { ListId = model.Id };
        //        //TODO: CORREGIR
        //        //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Create_Success"].Value);
        //    }

        //    return View("Edit", model);
        //}

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

                var updatedModelResult = await GetSampleListByIdAsync(sampleListViewModel.Id, "Edit", cancellationToken);


                if (updatedModelResult.IsSuccess)
                {
                    sampleListViewModel = updatedModelResult.Value.Adapt<EditSampleListViewModel>();
                    sampleListViewModel.NewItem = new SampleItemViewModel() { ListId = sampleListViewModel.Id };

                    if (result.IsFailure)
                    {
                        HandleFailureResult(result, "Edit");
                    }
                    else
                    {
                        //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value, elementId: "Edit");
                        AddSuccessMessage(localizer["Sample_SampleList_Edit_Success"].Value, "Edit");
                    }
                }
            }
            sampleListViewModel.NewItem = new SampleItemViewModel() { ListId = sampleListViewModel.Id };

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

            return View(model);
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

                if (result.IsFailure)
                {
                    //TODO: Traducir mensaje
                    //TODO: CORREGIR
                    //HandleErrorResult(result,
                    //                  ResponseMessageViewModel.Error(result.Exception.Message, "Do you want to delete anyway?"));
                    deleteSampleListViewModel.NeedsConfirmation = true;

                    return View(deleteSampleListViewModel);
                }

                //TODO: Cambiar mensaje
                //TODO: CORREGIR
                //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_Edit_Success"].Value);
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
                        sampleItemViewModel.ListId,
                        sampleItemViewModel.Adapt<SampleItemDto>()), cancellationToken: cancellationToken);

                if (addItemResult.IsFailure)
                {
                    HandleFailureResult(addItemResult, "AddItemValidations");
                }
                else
                {
                    //TODO: CORREGIR
                    //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_AddItem_Success"].Value)
                    //.SetId("Items");
                }
            }

            var result = await GetSampleListByIdAsync(sampleItemViewModel.ListId, "Edit", cancellationToken);
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

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int listId, int itemId, CancellationToken cancellationToken)
        {
            var removeResult = await mediator.Send(
                new RemoveSampleItemFromList.Command(
                    listId,
                    itemId), cancellationToken: cancellationToken);


            if (removeResult.IsFailure)
            {
                HandleFailureResult(removeResult);
            }
            else
            {
                //TODO: CORREGIR
                //ViewBag.ResponseMessage = ResponseMessageViewModel.Success(localizer["Sample_SampleList_RemoveItem_Success"].Value)
                //                                                  .SetId("Items");
            }


            var result = await GetSampleListByIdAsync(listId, "Edit", cancellationToken);
            var model = result.Value.Adapt<EditSampleListViewModel>();
            if (result.IsSuccess)
            {
                model.NewItem = new SampleItemViewModel() { ListId = model.Id };
            }

            return View("Edit", model);
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
