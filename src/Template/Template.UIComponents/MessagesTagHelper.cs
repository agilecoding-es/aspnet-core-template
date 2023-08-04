using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Template.UIComponents.Configuration;
using Template.UIComponents.Models;
using Template.Common.Extensions;
using static Template.UIComponents.Models.ResponseMessageViewModel;

namespace Template.UIComponents
{
    [HtmlTargetElement("div", Attributes = MessagesAttributeName)]
    public class MessagesTagHelper : TagHelper
    {

        public const string MessagesAttributeName = "asp-messages";

        private MessageType validationMessage;

        private readonly IHtmlGenerator generator;
        private readonly IOptions<Messages> options;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// If <see cref="MessageType.All"/>, <see cref="MessageType.Errors"/>, <see cref="MessageType.Validations"/>, <see cref="MessageType.Success"/> or <see cref="MessageType.Info"/>, appends a validation
        /// summary. Otherwise (<see cref="MessageType.None"/>, the default), this tag helper does nothing.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if setter is called with an invalid value.
        /// </exception>
        [HtmlAttributeName(MessagesAttributeName)]
        public MessageType ValidationMessage
        {
            get => validationMessage;
            set
            {
                switch (value)
                {
                    case MessageType.All:
                    case MessageType.Errors:
                    case MessageType.Validations:
                    case MessageType.Success:
                    case MessageType.Info:
                    case MessageType.None:
                        validationMessage = value;
                        break;

                    default:
                        //TODO: Localize
                        throw new ArgumentException("Invalid argument");
                }
            }
        }

        [HtmlAttributeName("id")]
        public string Id { get; set; }

        public MessagesTagHelper(IHtmlGenerator generator, IOptions<Messages> options)
        {
            this.generator = generator;
            this.options = options;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (ValidationMessage == MessageType.None)
            {
                return;
            }

            ResponseMessageViewModel failureResponse = null;
            if (ViewContext.TempData != null)
            {
                failureResponse = ViewContext.TempData[Constants.MESSAGE_RESPONSE] as ResponseMessageViewModel;
            }

            if (failureResponse != null && (failureResponse.ElementId == Id || string.IsNullOrEmpty(Id)))
            {
                var messages = GetMessagesByType(failureResponse);
                if (!messages.IsNullOrEmpty())
                {
                    AppendMessageItems(messages, output.Content);
                }
            }
        }

        private void AppendMessageItems(List<Message> messages, TagHelperContent content)
        {

            TagBuilder svgContainer = new TagBuilder("svg");
            svgContainer.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
            svgContainer.Attributes.Add("style", "display: none;");

            //SVG EXCLAMATION SYMBOL - START
            TagBuilder svgSymbolExclamation = new TagBuilder("symbol");
            svgSymbolExclamation.Attributes.Add("id", "exclamation-triangle-fill");
            svgSymbolExclamation.Attributes.Add("viewBox", "0 0 16 16");

            TagBuilder svgSymbolExclamationPath = new TagBuilder("path");
            svgSymbolExclamationPath.Attributes.Add("d", "M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z");

            svgSymbolExclamation.InnerHtml.AppendHtml(svgSymbolExclamationPath);
            //SVG EXCLAMATION SYMBOL - END

            //SVG SUCCESS SYMBOL - START
            TagBuilder svgSymbolSucess = new TagBuilder("symbol");
            svgSymbolSucess.Attributes.Add("id", "check-circle-fill");
            svgSymbolSucess.Attributes.Add("viewBox", "0 0 16 16");

            TagBuilder svgSymbolSucessPath = new TagBuilder("path");
            svgSymbolSucessPath.Attributes.Add("d", "M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z");

            svgSymbolSucess.InnerHtml.AppendHtml(svgSymbolSucessPath);
            //SVG SUCCESS SYMBOL - END

            //SVG INFO SYMBOL - START
            TagBuilder svgSymbolInfo = new TagBuilder("symbol");
            svgSymbolInfo.Attributes.Add("id", "info-fill");
            svgSymbolInfo.Attributes.Add("viewBox", "0 0 16 16");

            TagBuilder svgSymbolInfoPath = new TagBuilder("path");
            svgSymbolInfoPath.Attributes.Add("d", "M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z");

            svgSymbolInfo.InnerHtml.AppendHtml(svgSymbolInfoPath);
            //SVG INFO SYMBOL - END

            svgContainer.InnerHtml.AppendHtml(svgSymbolExclamation);
            svgContainer.InnerHtml.AppendHtml(svgSymbolSucess);
            svgContainer.InnerHtml.AppendHtml(svgSymbolInfo);
            content.AppendHtml(svgContainer);

            foreach (var message in messages)
            {
                var messageType = GetMessagesType(message.MessageType);
                TagBuilder messageItemContainer = new TagBuilder("div");
                messageItemContainer.Attributes.Add("role", "alert");
                messageItemContainer.Attributes.Add($"ui-{message.Id}", null);
                messageItemContainer.AddCssClass($"alert alert-{messageType} d-flex align-items-center");

                TagBuilder svgExclamation = new TagBuilder("svg");
                svgExclamation.Attributes.Add("role", "img");
                svgExclamation.Attributes.Add("aria-label", $"{messageType}:");
                svgExclamation.Attributes.Add("style", "width: 1em; height: 1em;");
                svgExclamation.AddCssClass("flex-shrink-0 me-2");

                TagBuilder svgExclamationUse = new TagBuilder("use");
                svgExclamationUse.Attributes.Add("xlink:href", "#exclamation-triangle-fill");
                svgExclamation.InnerHtml.AppendHtml(svgExclamationUse);

                TagBuilder messageItemTag = new TagBuilder("div");
                if (!string.IsNullOrEmpty(message.Title))
                {
                    TagBuilder messageItemStrong = new TagBuilder("strong");
                    messageItemStrong.AddCssClass("pe-1");
                    messageItemStrong.InnerHtml.SetContent(message.Title);
                    messageItemTag.InnerHtml.AppendHtml(messageItemStrong);
                }
                messageItemTag.InnerHtml.AppendHtml(message.Content);

                TagBuilder messageItemButtonClose = new TagBuilder("button");
                messageItemButtonClose.Attributes.Add("type", "button");
                messageItemButtonClose.Attributes.Add("data-bs-dismiss", "alert");
                messageItemButtonClose.Attributes.Add("aria-label", "close");
                messageItemButtonClose.AddCssClass("btn-close ms-auto p-2");

                TagBuilder messageItemButtonCloseSpan = new TagBuilder("span");
                messageItemButtonCloseSpan.Attributes.Add("aria-hidden", "true");
                messageItemButtonClose.InnerHtml.AppendHtml(messageItemButtonCloseSpan);


                messageItemContainer.InnerHtml.AppendHtml(svgExclamation);
                messageItemContainer.InnerHtml.AppendHtml(messageItemTag);
                messageItemContainer.InnerHtml.AppendHtml(messageItemButtonClose);

                content.AppendHtml(messageItemContainer);
            }
        }

        private string GetMessagesType(ResponseType messageType)
        {
            switch (messageType)
            {
                case ResponseType.Info:
                    return "primary";
                case ResponseType.Success:
                    return "success";
                case ResponseType.Validation:
                    return "warning";
                case ResponseType.Error:
                default:
                    return "danger";
            }
        }

        private List<Message> GetMessagesByType(ResponseMessageViewModel failureResponse)
        {
            switch (ValidationMessage)
            {
                case MessageType.Errors:
                    return failureResponse.Messages
                        .Where(message => message.MessageType == ResponseType.Error)
                        .ToList();
                case MessageType.Validations:
                    return failureResponse.Messages
                        .Where(message => message.MessageType == ResponseType.Validation)
                        .ToList();
                case MessageType.Success:
                    return failureResponse.Messages
                        .Where(message => message.MessageType == ResponseType.Success)
                        .ToList();
                case MessageType.Info:
                    return failureResponse.Messages
                        .Where(message => message.MessageType == ResponseType.Info)
                        .ToList();

                case MessageType.All:
                default:
                    return failureResponse.Messages.ToList();
            }
        }


        /// <summary>
        /// Acceptable message types rendering modes.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// No messages.
            /// </summary>
            None,

            /// <summary>
            /// Messages with response-type errors only (excludes all property errors).
            /// </summary>
            Errors,

            /// <summary>
            /// Messages with response-type validations only (excludes all property errors).
            /// </summary>
            Validations,


            /// <summary>
            /// Messages with response-type success only.
            /// </summary>
            Success,


            /// <summary>
            /// Messages with response-type info only.
            /// </summary>
            Info,

            /// <summary>
            /// All types of messages.
            /// </summary>
            All
        }
    }
}
