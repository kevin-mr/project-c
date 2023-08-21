namespace ProjectC.Shared.Models
{
    public class WebhookRuleDto
    {
        public WebhookRuleDto()
        {
            Id = 0;
            Method = WebhookRuleMethodDto.POST;
            Path = string.Empty;
            Description = string.Empty;
            RedirectUrl = string.Empty;
            WebhookRuleEvents = new List<RequestEventDto>();
            WebhookEvents = new List<WebhookEventDto>();
        }

        public int Id { get; set; }
        public WebhookRuleMethodDto Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string? RedirectUrl { get; set; }

        public string MethodLabel => Method.ToString();

        public IEnumerable<RequestEventDto> WebhookRuleEvents { get; set; }
        public IEnumerable<WebhookEventDto> WebhookEvents { get; set; }
    }
}
