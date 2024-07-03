namespace ProjectC.Shared.Models
{
    public class CreateRequestRuleTriggerDto
    {
        public CreateRequestRuleTriggerDto()
        {
            Description = string.Empty;
        }

        public string Description { get; set; }

        public int RequestRuleVariantId { get; set; }
        public int WebhookEventId { get; set; }
    }
}
