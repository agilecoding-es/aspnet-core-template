﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Reflection.Metadata;
using Template.Application.Exceptions;
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

        public IActionResult HandleErrorResult(Result result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            if (Request.IsAjaxRequest())
            {
                if (result.Exception is ValidationException || result.Exception is DomainException)
                {   
                    //TODO: Agregar severidad a ValidationException para poder emitir warnings
                    return Json(ResponseMessageViewModel.Error(result.Exception.Message));
                }
                else
                {
                    //TODO: Agregar severidad a ValidationException para poder emitir warnings
                    return Json(ResponseMessageViewModel.Error(localizer["Shared_Message_ErrorOccured"].Value));
                }
            }
            else
            {
                if (result.Exception is ValidationException || result.Exception is DomainException)
                {
                    //TODO: Agregar severidad a ValidationException para poder emitir warnings
                    ViewBag.ResponseMessage = ResponseMessageViewModel.Error(result.Exception.Message);
                }
                else
                {
                    //TODO: Agregar severidad a ValidationException para poder emitir warnings
                    ViewBag.ResponseMessage = ResponseMessageViewModel.Error(localizer["Shared_Message_ErrorOccured"].Value);
                }
            }

            return null;
        }

    }
}
