using System.ComponentModel;

namespace Template.MvcWebApp.Models
{
    public class ResponseMessageViewModel
    {
        public enum ResponseType
        {
            [Description("success")]
            Success,
            [Description("warning")]
            Warning,
            [Description("danger")]
            Error
        }

        public ResponseType MessageType { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Id { get; private set; }

        public ResponseMessageViewModel SetId(string id)
        {
            Id = id;
            return this;
        }

        public static ResponseMessageViewModel Success(string title, string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Success, Title = title, Content = content };
        public static ResponseMessageViewModel Success(string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Success, Content = content };

        public static ResponseMessageViewModel Warning(string title, string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Warning, Title = title, Content = content };
        public static ResponseMessageViewModel Warning(string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Warning, Content = content };

        public static ResponseMessageViewModel Error(string title, string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Error, Title = title, Content = content };
        public static ResponseMessageViewModel Error(string content) => new ResponseMessageViewModel() { MessageType = ResponseType.Error, Content = content };
    }
}
