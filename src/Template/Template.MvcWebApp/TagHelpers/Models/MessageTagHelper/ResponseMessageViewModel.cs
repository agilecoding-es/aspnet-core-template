using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Template.MvcWebApp.TagHelpers.Models.MessageTagHelper
{
    [Serializable]
    public class ResponseMessageViewModel
    {
        public class Message
        {
            public string Id { get; set; }
            public ResponseType MessageType { get; set; }
            public string Content { get; set; }
            public string Title { get; set; }

            public Message(string id, ResponseType messageType, string content, string title = null)
            {
                Id = id;
                MessageType = messageType;
                Content = content;
                Title = title;
            }
        }

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


        private ResponseMessageViewModel() { }

        [JsonConstructor]
        public ResponseMessageViewModel(string elementId, List<Message> messages)
        {
            ElementId = elementId;
            this.messages = messages ?? new List<Message>();
        }

        public static ResponseMessageViewModel Create(string elementId = null) => new ResponseMessageViewModel() { ElementId = elementId };

        public void AddErrorMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Error, content, title));
        public void AddValidationMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Validation, content, title));
        public void AddSuccessMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Success, content, title));
        public void AddInfoMessage(string content, string title = null) => messages.Add(new Message(Guid.NewGuid().ToString(), ResponseType.Info, content, title));
    }
}
