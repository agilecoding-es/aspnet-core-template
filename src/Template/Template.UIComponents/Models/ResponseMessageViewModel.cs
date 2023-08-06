﻿using System.Collections.ObjectModel;

namespace Template.UIComponents.Models
{
    public class ResponseMessageViewModel
    {
        public record Message(string Id, ResponseType MessageType, string Content, string Title = null);

        public enum ResponseType
        {
            Success,
            Info,
            Validation,
            Error
        }

        private List<Message> messages = new List<Message>();

        public string ElementId { get; private set; }
        public ReadOnlyCollection<Message> Messages => messages.AsReadOnly();

        public static ResponseMessageViewModel Create(string elementId = null) => new ResponseMessageViewModel() { ElementId = elementId };

        public void AddErrorMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(),ResponseType.Error, content, title));
        public void AddValidationMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Validation, content, title));
        public void AddSuccessMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Success, content, title));
        public void AddInfoMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Info, content, title));
    }
}
