using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Reflection.Metadata;
using Template.Application.Exceptions;
using Template.Common;
using Template.Configuration;
using Template.Domain.Entities.Shared;
using Template.Domain.Exceptions;
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

        public ResponseMessageViewModel HandleErrorResult(Result result, string id = null)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            return HandleErrorResult(result, ResponseMessageViewModel.Error(result.Exception.Message), id);
        }

        public ResponseMessageViewModel HandleErrorResult(Result result, ResponseMessageViewModel validationResponse, string id = null)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));
            ResponseMessageViewModel response = validationResponse;
            
            if (!(result.Exception is ValidationException || result.Exception is DomainException))
            {
                //TODO: Agregar severidad a ValidationException para poder emitir warnings
                response = ResponseMessageViewModel.Error(localizer["Shared_Message_ErrorOccured"].Value);
            }

            response.SetId(id);

            //if (!Request.IsAjaxRequest())
            //{
            //    //TODO: Agregar severidad a ValidationException para poder emitir warnings
            //    ViewBag.ResponseMessage = response;
            //}

            ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, response.Content);
            
            return response;
        }

    }
}
