import { Enums } from '../enums'

export class MessageHelper {


    constructor() {

    }

    showMesssage(content) {
        try {
            if (!content) return;

            const data = JSON.parse(content);

            if (
                (data.ElementId == null || typeof data.ElementId === 'string') &&
                Array.isArray(data.Messages)
            ) {
                let container = getContainer(data);
                addMessageItems(container, data)
            } else {
                console.log('JSON data does not have the expected structure.');
            }
        } catch (error) {
            console.error('Error parsing JSON:', error);
        }
    }
}

function getContainer(data) {
    let container = null;
    if (data.ElementId) {
        container = document.querySelector(`#${data.ElementId}`);
    }
    else {
        container = document.querySelector('.message-component');
    }
    return container;
}

function addMessageItems(container, data) {
    const $container = $(container);
    const messages = data.Messages;
    let delay = $container.data('hiding-delay');

    for (let i = 0; i < messages.length; i++) {
        var messageItem = addMessageItem(messages[i], delay);
        $container.append(messageItem);

        hideMessageElements(container, messageItem);
    }
}

function addMessageItem(message, delay) {
    const messageType = getMessagesType(message.MessageType);

    const messageItemContainer = document.createElement("div");
    messageItemContainer.setAttribute("role", "alert");
    messageItemContainer.setAttribute(`ui-${message.Id}`, null);
    messageItemContainer.setAttribute("styles", "position: relative;");
    messageItemContainer.classList.add("message-item", "alert", `alert-${messageType}`, "d-flex", "align-items-center");

    var [img, alt] = getMessagesImage(message.MessageType);

    var image = document.createElement("img");
    image.setAttribute("src", `/Content/images/ui-components/message-taghelper/${img}.svg`);
    image.setAttribute("alt", alt);
    image.setAttribute("role", "img");
    image.setAttribute("aria-label", `${messageType}:`);
    image.setAttribute("style", "width: 1em; height: 1em;");
    image.classList.add("flex-shrink-0", "me-2");

    var messageItemTag = document.createElement("div");
    if (message.Title != null && message.Title != '') {
        var messageItemStrong = document.createElement("strong");
        messageItemStrong.classList.add("pe-1");
        messageItemStrong.InnerHtml.SetContent(message.Title);
        messageItemTag.append(messageItemStrong);
    }
    messageItemTag.append(message.Content);

    var messageItemButtonClose = document.createElement("button");
    messageItemButtonClose.setAttribute("type", "button");
    messageItemButtonClose.setAttribute("data-bs-dismiss", "alert");
    messageItemButtonClose.setAttribute("aria-label", "close");
    messageItemButtonClose.classList.add("btn-close", "ms-auto", "p-2");

    var messageItemButtonCloseSpan = document.createElement("span");
    messageItemButtonCloseSpan.setAttribute("aria-hidden", "true");
    messageItemButtonClose.append(messageItemButtonCloseSpan);

    messageItemContainer.append(image);
    messageItemContainer.append(messageItemTag);
    messageItemContainer.append(messageItemButtonClose);

    if (delay > 0) {
        var progressBar = document.createElement("div");
        progressBar.classList.add("message-progress", `bg-${messageType}`);

        messageItemContainer.append(progressBar);
    }
    return messageItemContainer;
}

function getMessagesType(messageType) {
    switch (messageType) {
        case Enums.ResponseType.Info:
            return "primary";
        case Enums.ResponseType.Success:
            return "success";
        case Enums.ResponseType.Validation:
            return "warning";
        case Enums.ResponseType.Error:
        default:
            return "danger";
    }
}

function getMessagesImage(messageType) {
    switch (messageType) {
        case Enums.ResponseType.Info:
            return ["info", "info image"];
        case Enums.ResponseType.Success:
            return ["success", "success image"];
        case Enums.ResponseType.Validation:
        case Enums.ResponseType.Error:
        default:
            return ["exclamation", "exclamation image"];
    }
}

function hideMessageElements(container, element) {
    let delay = $(container).data('hiding-delay');
    let timeout = delay * 1000;

    if (timeout) {
        let progressBar = $(element).find('.message-progress');
        let increment = 10 / delay;
        let currentWidth = 0;

        let intervalId = setInterval(function () {
            if (currentWidth + increment < 100) {
                currentWidth += increment;
                progressBar.css('width', currentWidth + '%');
            } else {
                progressBar.css('width', '100%');
            }
        }, 100);

        setTimeout(function () {
            clearInterval(intervalId);
            $(element).fadeOut('slow', function () {
                $(this).remove();
            });
        }, timeout);
    }
}