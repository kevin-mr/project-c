namespace ProjectC.Shared.Models
{
    public class RequestRuleTriggerDto
    {
        public RequestRuleTriggerDto()
        {
            Id = 0;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string? WebhookRuleDescription { get; set; }

        public int RequestRuleVariantId { get; set; }
        public int WebhookEventId { get; set; }
    }
}
