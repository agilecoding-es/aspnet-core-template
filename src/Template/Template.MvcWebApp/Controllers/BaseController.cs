using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Exceptions;
using Template.Common.Extensions;
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
                responseMessage = (TempData[TempDataKey.MESSAGE_RESPONSE] as string).Deserialize<ResponseMessageViewModel>();

            if (!(result.Exception is ValidationException || result.Exception is DomainException))
            {
                responseMessage.AddErrorMessage(localizer.GetString("Shared_Message_ErrorOccured"));
            }
            else
            {
                //ModelState.AddModelError(Constants.KeyErrors.ValidationError, result.Exception.Message);
                responseMessage.AddValidationMessage(result.Exception.Message);
            }

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage.Serialize();
        }

        public void AddSuccessMessage(string message, string elementId = null)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));

            ResponseMessageViewModel responseMessage = null;
            if (TempData[TempDataKey.MESSAGE_RESPONSE] == null)
                responseMessage = ResponseMessageViewModel.Create(elementId);
            else
                responseMessage = (TempData[TempDataKey.MESSAGE_RESPONSE] as string).Deserialize<ResponseMessageViewModel>();

            responseMessage.AddSuccessMessage(message);

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage.Serialize();
        }

        public void AddInfoMessage(string message, string elementId = null)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));

            ResponseMessageViewModel responseMessage = null;
            if (TempData[TempDataKey.MESSAGE_RESPONSE] == null)
                responseMessage = ResponseMessageViewModel.Create(elementId);
            else
                responseMessage = (TempData[TempDataKey.MESSAGE_RESPONSE] as string).Deserialize<ResponseMessageViewModel>();

            responseMessage.AddInfoMessage(message);

            TempData[TempDataKey.MESSAGE_RESPONSE] = responseMessage.Serialize();
        }
    }
}
