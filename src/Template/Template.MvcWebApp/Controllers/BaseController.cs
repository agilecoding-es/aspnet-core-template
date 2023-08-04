using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Exceptions;
using Template.Domain.Entities.Shared;
using Template.Domain.Exceptions;
using Template.MvcWebApp.Common;
using Template.UIComponents.Models;

namespace Template.MvcWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IHtmlLocalizer localizer;

        public BaseController(IHtmlLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public void HandleFailureResult(Result result, string elementId = null)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            ResponseMessageViewModel responseMessage = null;
            if (TempData[TempDataKey.MESSAGE_RESPONSE] == null)
                responseMessage = ResponseMessageViewModel.Create(elementId);
            else
                responseMessage = TempData[TempDataKey.MESSAGE_RESPONSE] as ResponseMessageViewModel;

            if (!(result.Exception is ValidationException || result.Exception is DomainException))
            {
                responseMessage.AddErrorMessage(localizer.GetString("Shared_Message_ErrorOccured"));
            }
            else
            {
                //ModelState.AddModelError(Constants.KeyErrors.ValidationError, result.Exception.Message);
                responseMessage.AddValidationMessage(result.Exception.Message);
            }

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage;
        }

        public void AddSuccessMessage(string message, string elementId = null)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));

            ResponseMessageViewModel responseMessage = null;
            if (TempData[TempDataKey.MESSAGE_RESPONSE] == null)
                responseMessage = ResponseMessageViewModel.Create(elementId);
            else
                responseMessage = TempData[TempDataKey.MESSAGE_RESPONSE] as ResponseMessageViewModel;

            responseMessage.AddSuccessMessage(message,"Test");

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage;
        }

        public void AddInfoMessage(string message, string elementId = null)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));

            ResponseMessageViewModel responseMessage = null;
            if (TempData[TempDataKey.MESSAGE_RESPONSE] == null)
                responseMessage = ResponseMessageViewModel.Create(elementId);
            else
                responseMessage = TempData[TempDataKey.MESSAGE_RESPONSE] as ResponseMessageViewModel;

            responseMessage.AddInfoMessage(message);

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage;
        }

        //public IActionResult HandleErrorResponse(Result result, string elementId = null)
        //{
        //    _ = result ?? throw new ArgumentNullException(nameof(result));

        //    if (!string.IsNullOrEmpty(elementId))
        //    {
        //        TempData[TempDataKey.ERROR_ID] = elementId;
        //    }

        //    if (!(result.Exception is ValidationException || result.Exception is DomainException))
        //    {
        //        //TODO: Agregar severidad a ValidationException para poder emitir warnings
        //        ModelState.AddModelError(Constants.KeyErrors.GenericError, localizer.GetString("Shared_Message_ErrorOccured"));

        //        return BadRequest(ModelState);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(Constants.KeyErrors.ValidationError, result.Exception.Message);
        //        return NotFound(ModelState);
        //    }
        //}
    }
}
