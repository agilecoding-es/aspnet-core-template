using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Template.ExternalServices.EmailService.DTOs
{
    public class EmailTemplateDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
        
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
