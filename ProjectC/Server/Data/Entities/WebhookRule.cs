namespace ProjectC.Server.Data.Entities
{
    public class WebhookRule
    {
        public WebhookRule()
        {
            Id = 0;
            Method = WebhookRuleMethod.POST;
            Path = string.Empty;
            Description = string.Empty;
            RedirectUrl = string.Empty;
            RequestEvents = new List<RequestEvent>();
        }

        public int Id { get; set; }
        public WebhookRuleMethod Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string? RedirectUrl { get; set; }

        public IEnumerable<RequestEvent> RequestEvents { get; set; }
    }
}
