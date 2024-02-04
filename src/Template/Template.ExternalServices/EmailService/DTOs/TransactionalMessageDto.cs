using System.Text.Json.Serialization;

namespace Template.ExternalServices.EmailService.DTOs
{
    public class TransactionalMessageDto<TData>
    {
        [JsonPropertyName("subscriber_email")]
        public string SubscriberEmail { get; set; }

        [JsonPropertyName("subscriber_id")]
        public int SubscriberId { get; set; }

        [JsonPropertyName("subscriber_emails")]
        public string[] SubscriberEmails { get; set; }

        [JsonPropertyName("subscriber_ids")]
        public int[] SubscriberIds { get; set; }

        [JsonPropertyName("template_id")]
        public int TemplateId { get; set; }

        [JsonPropertyName("from_email")]
        public string FromEmail { get; set; }

        [JsonPropertyName("data")]
        public TData Data { get; set; }

        [JsonPropertyName("headers")]
        public string[] Headers { get; set; }

        [JsonPropertyName("messenger")]
        public string Messenger { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
    }
}
