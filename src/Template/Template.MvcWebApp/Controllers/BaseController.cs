using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata;
using Template.Application.Exceptions;
using Template.Common;
using Template.Configuration;
using Template.Domain.Entities.Shared;
using Template.Domain.Exceptions;
using Template.MvcWebApp.Common;
using Template.MvcWebApp.Extensions;
using Template.MvcWebApp.Models;

namespace Template.MvcWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IHtmlLocalizer localizer;

        public BaseController(IHtmlLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public void HandleErrorResult(Result result, string elementId = null)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            if (!string.IsNullOrEmpty(elementId))
            {
                TempData[TempDataKey.ERROR_ID] = elementId;
            }

            if (!(result.Exception is ValidationException || result.Exception is DomainException))
            {
                //TODO: Agregar severidad a ValidationException para poder emitir warnings
                ModelState.AddModelError(Constants.KeyErrors.GenericError, localizer.GetString("Shared_Message_ErrorOccured"));
            }
            else
            {
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, result.Exception.Message);
            }
        }

        public IActionResult HandleErrorResponse(Result result, string elementId = null)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            if (!string.IsNullOrEmpty(elementId))
            {
                TempData[TempDataKey.ERROR_ID] = elementId;
            }

            if (!(result.Exception is ValidationException || result.Exception is DomainException))
            {
                //TODO: Agregar severidad a ValidationException para poder emitir warnings
                ModelState.AddModelError(Constants.KeyErrors.GenericError, localizer.GetString("Shared_Message_ErrorOccured"));

                return BadRequest(ModelState);
            }
            else
            {
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, result.Exception.Message);
                return NotFound(ModelState);
            }
        }
    }
}
