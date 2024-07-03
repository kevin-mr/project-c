namespace ProjectC.Shared.Models
{
    public class WebhookEventDto : IRequestEventDto
    {
        public WebhookEventDto()
        {
            Id = 0;
            Method = string.Empty;
            Headers = new Dictionary<string, string>();
            JsonHeaders = string.Empty;
            Body = string.Empty;
            JsonBody = string.Empty;
            Path = string.Empty;
            RequestRuleTriggers = new List<RequestRuleTriggerDto>();
        }

        public int Id { get; set; }
        public string Method { get; set; }
        public string JsonHeaders { get; set; }
        public string JsonBody { get; set; }
        public string Path { get; set; }
        public string? RedirectUrl { get; set; }
        public string? WebhookRuleDescription { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public int? WebhookRuleId { get; set; }
        public IEnumerable<RequestRuleTriggerDto> RequestRuleTriggers { get; set; }
    }
}
