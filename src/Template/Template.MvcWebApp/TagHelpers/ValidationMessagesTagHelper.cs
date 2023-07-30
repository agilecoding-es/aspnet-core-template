using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using Template.Common;
using Template.Common.Extensions;
using Template.Domain;

namespace Template.MvcWebApp.TagHelpers
{
    [HtmlTargetElement("div", Attributes = ValidationMessageAttributeName)]
    public class ValidationMessagesTagHelper : TagHelper
    {
        public const string ValidationMessageAttributeName = "asp-validation-messages";
        private ValidationMessage _validationMessage;

        private readonly IHtmlGenerator _generator;


        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// If <see cref="ValidationMessage.All"/> or <see cref="ValidationMessage.Errors"/> or <see cref="ValidationMessage.Validations"/>, appends a validation
        /// summary. Otherwise (<see cref="ValidationMessage.None"/>, the default), this tag helper does nothing.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if setter is called with an invalid value.
        /// </exception>
        [HtmlAttributeName(ValidationMessageAttributeName)]
        public ValidationMessage ValidationMessage
        {
            get => _validationMessage;
            set
            {
                switch (value)
                {
                    case ValidationMessage.All:
                    case ValidationMessage.Errors:
                    case ValidationMessage.Validations:
                    case ValidationMessage.None:
                        _validationMessage = value;
                        break;

                    default:
                        //TODO: Localize
                        throw new ArgumentException("Invalid argument");
                }
            }
        }
        public ValidationMessagesTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (ValidationMessage == ValidationMessage.None)
            {
                return;
            }

            var messages = GetMessagesByType();
            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    AppendMessageItems(message.Key, message.Value.Errors, output.Content);
                }

            }
        }

        private void AppendMessageItems(string key, ModelErrorCollection errors, TagHelperContent content)
        {

            TagBuilder svgContainer = new TagBuilder("svg");
            svgContainer.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
            svgContainer.Attributes.Add("style", "display: none;");

            TagBuilder svgSymbolExclamation = new TagBuilder("symbol");
            svgSymbolExclamation.Attributes.Add("id", "exclamation-triangle-fill");
            svgSymbolExclamation.Attributes.Add("viewBox", "0 0 16 16");

            TagBuilder svgSymbolExclamationPath = new TagBuilder("path");
            svgSymbolExclamationPath.Attributes.Add("d", "M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z");

            svgSymbolExclamation.InnerHtml.AppendHtml(svgSymbolExclamationPath);
            svgContainer.InnerHtml.AppendHtml(svgSymbolExclamation);
            content.AppendHtml(svgContainer);

            foreach (var error in errors)
            {
                var messageType = GetMessagesType(key);
                TagBuilder messageItemContainer = new TagBuilder("div");
                messageItemContainer.Attributes.Add("role", "alert");
                messageItemContainer.AddCssClass($"alert alert-{messageType} d-flex align-items-center");

                TagBuilder svgExlamation = new TagBuilder("svg");
                svgExlamation.Attributes.Add("role", "img");
                svgExlamation.Attributes.Add("aria-label", $"{messageType}:");
                svgExlamation.Attributes.Add("style", "width: 1em; height: 1em;");
                svgExlamation.AddCssClass("flex-shrink-0 me-2");
                
                TagBuilder svgExlamationUse = new TagBuilder("use");
                svgExlamationUse.Attributes.Add("xlink:href", "#exclamation-triangle-fill");
                svgExlamation.InnerHtml.AppendHtml(svgExlamationUse);

                TagBuilder messageItemTag = new TagBuilder("div");
                messageItemTag.InnerHtml.SetContent(error.ErrorMessage);

                TagBuilder messageItemButtonClose = new TagBuilder("button");
                messageItemButtonClose.Attributes.Add("type", "button");
                messageItemButtonClose.Attributes.Add("data-bs-dismiss", "alert");
                messageItemButtonClose.Attributes.Add("aria-label", "close");
                messageItemButtonClose.AddCssClass("btn-close ms-auto p-2");

                TagBuilder messageItemButtonCloseSpan = new TagBuilder("span");
                messageItemButtonCloseSpan.Attributes.Add("aria-hidden", "true");
                messageItemButtonClose.InnerHtml.AppendHtml(messageItemButtonCloseSpan);


                messageItemContainer.InnerHtml.AppendHtml(svgExlamation);
                messageItemContainer.InnerHtml.AppendHtml(messageItemTag);
                messageItemContainer.InnerHtml.AppendHtml(messageItemButtonClose);

                content.AppendHtml(messageItemContainer);
            }
        }

        private string GetMessagesType(string key)
        {
            switch (key)
            {
                case Constants.KeyErrors.ValidationError:
                    return "warning";
                case Constants.KeyErrors.GenericError:
                default:
                    return "danger";

            }
        }

        private List<KeyValuePair<string, ModelStateEntry>> GetMessagesByType()
        {
            switch (ValidationMessage)
            {
                case ValidationMessage.Errors:
                    return ViewContext.ModelState
                        .Where(entry => entry.Key == Constants.KeyErrors.GenericError)
                        .Select(entry => entry)
                        .ToList();
                case ValidationMessage.Validations:
                    return ViewContext.ModelState
                        .Where(entry => entry.Key == Constants.KeyErrors.ValidationError)
                        .Select(entry => entry)
                        .ToList();

                case ValidationMessage.All:
                default:
                    return ViewContext.ModelState
                        .Where(entry => entry.Key == Constants.KeyErrors.GenericError || entry.Key == Constants.KeyErrors.ValidationError)
                        .Select(entry => entry)
                        .ToList();
            }
        }
    }
}
