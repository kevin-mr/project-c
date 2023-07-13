using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ProjectC.Server.Data.Entities
{
    public class RequestEvent
    {
        public RequestEvent()
        {
            Id = 0;
            Method = string.Empty;
            JsonHeaders = string.Empty;
            JsonBody = string.Empty;
            ArrivalDate = DateTime.Now;
            ForWorkflowAction = false;
        }

        public int Id { get; set; }
        public string Method { get; set; }
        public string JsonHeaders { get; set; }
        public string JsonBody { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool ForWorkflowAction { get; set; }

        public int? RequestRuleId { get; set; }
        public RequestRule? RequestRule { get; set; }
        public int? WebhookRuleId { get; set; }
        public WebhookRule? WebhookRule { get; set; }

        public Dictionary<string, string> Headers =>
            JsonSerializer.Deserialize<Dictionary<string, string>>(JsonHeaders)
            ?? new Dictionary<string, string>();
        public string Body => JsonSerializer.Deserialize<string>(JsonBody) ?? string.Empty;
    }
}
