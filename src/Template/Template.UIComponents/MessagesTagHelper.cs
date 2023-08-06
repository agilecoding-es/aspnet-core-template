using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Template.Common.Extensions;
using Template.UIComponents.Configuration;
using Template.UIComponents.Models;
using static Template.UIComponents.Models.ResponseMessageViewModel;

namespace Template.UIComponents
{
    [HtmlTargetElement("div", Attributes = MessagesAttributeName)]
    public class MessagesTagHelper : TagHelper
    {

        public const string MessagesAttributeName = "asp-messages";

        private MessageType validationMessage;
        private readonly IHtmlGenerator generator;
        private readonly IHtmlHelper htmlHelper;
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

        public int HidingDelay { get; set; } = 0;

        public MessagesTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper, IOptions<Messages> options)
        {
            this.generator = generator;
            this.htmlHelper = htmlHelper;
            this.options = options;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (ValidationMessage == MessageType.None)
            {
                return;
            }

            (htmlHelper as IViewContextAware).Contextualize(ViewContext);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

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

            var partialViewResult = await htmlHelper.PartialAsync("~/Views/Shared/_MessageTagHelperScripts.cshtml");
            output.Content.AppendHtml(partialViewResult);
        }

        private void AppendMessageItems(List<Message> messages, TagHelperContent content)
        {
            foreach (var message in messages)
            {
                var messageType = GetMessagesType(message.MessageType);

                TagBuilder messageItemContainer = new TagBuilder("div");
                messageItemContainer.Attributes.Add("role", "alert");
                messageItemContainer.Attributes.Add($"ui-{message.Id}", null);
                messageItemContainer.Attributes.Add("styles", "position: relative;");
                messageItemContainer.Attributes.Add($"data-hiding-delay", HidingDelay.ToString());
                messageItemContainer.AddCssClass($"alert alert-{messageType} d-flex align-items-center");

                var (img, alt) = GetMessagesImage(message.MessageType);

                TagBuilder image = new TagBuilder("img");
                image.Attributes.Add("src", $"/UIComponents/message-taghelper/images/{img}.svg");
                image.Attributes.Add("alt", alt);
                image.Attributes.Add("role", "img");
                image.Attributes.Add("aria-label", $"{messageType}:");
                image.Attributes.Add("style", "width: 1em; height: 1em;");
                image.AddCssClass("flex-shrink-0 me-2");

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

                TagBuilder progressBar = new TagBuilder("div");
                progressBar.AddCssClass($"message-progress bg-{messageType}");

                messageItemContainer.InnerHtml.AppendHtml(image);
                messageItemContainer.InnerHtml.AppendHtml(messageItemTag);
                messageItemContainer.InnerHtml.AppendHtml(messageItemButtonClose);

                if (HidingDelay > 0)
                {
                    messageItemContainer.InnerHtml.AppendHtml(progressBar);
                }

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

        private (string Img, string Alt) GetMessagesImage(ResponseType messageType)
        {
            switch (messageType)
            {
                case ResponseType.Info:
                    return ("info", "info image");
                case ResponseType.Success:
                    return ("success", "success image");
                case ResponseType.Validation:
                case ResponseType.Error:
                default:
                    return ("exclamation", "exclamation image");
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
