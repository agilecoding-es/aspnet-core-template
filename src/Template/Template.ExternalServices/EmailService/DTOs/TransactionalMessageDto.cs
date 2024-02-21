using System.Text.Json.Serialization;

namespace Template.ExternalServices.EmailService.DTOs
{
    public class TransactionalMessageDto<TData>
    {
        [JsonPropertyName("subscriber_email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SubscriberEmail { get; set; }

        [JsonPropertyName("subscriber_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int SubscriberId { get; set; }

        [JsonPropertyName("subscriber_emails")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] SubscriberEmails { get; set; }

        [JsonPropertyName("subscriber_ids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int[] SubscriberIds { get; set; }

        [JsonPropertyName("template_id")]
        public int TemplateId { get; set; }

        [JsonPropertyName("from_email")]
        public string FromEmail { get; set; }

        [JsonPropertyName("data")]
        public TData Data { get; set; }

        [JsonPropertyName("headers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] Headers { get; set; }

        [JsonPropertyName("messenger")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Messenger { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
    }
}
