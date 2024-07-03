namespace ProjectC.Server.Data.Entities
{
    public class RequestRuleTrigger
    {
        public RequestRuleTrigger()
        {
            Id = 0;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public int RequestRuleVariantId { get; set; }
        public RequestRuleVariant? RequestRuleVariant { get; set; }
        public int WebhookEventId { get; set; }
        public WebhookEvent? WebhookEvent { get; set; }
    }
}
