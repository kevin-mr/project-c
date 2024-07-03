using System.Text.Json;

namespace ProjectC.Server.Data.Entities
{
    public class RequestEvent
    {
        public RequestEvent()
        {
            Id = 0;
            Path = string.Empty;
            Method = string.Empty;
            JsonHeaders = string.Empty;
            JsonBody = string.Empty;
            ArrivalDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string JsonHeaders { get; set; }
        public string JsonBody { get; set; }
        public DateTime ArrivalDate { get; set; }

        public int? RequestRuleVariantId { get; set; }
        public RequestRuleVariant? RequestRuleVariant { get; set; }
        public int? RequestRuleId { get; set; }
        public RequestRule? RequestRule { get; set; }
        public int? WebhookRuleId { get; set; }
        public WebhookRule? WebhookRule { get; set; }

        public Dictionary<string, string> Headers =>
            JsonSerializer.Deserialize<Dictionary<string, string>>(JsonHeaders)
            ?? new Dictionary<string, string>();
        public string Body => JsonBody;
    }
}
