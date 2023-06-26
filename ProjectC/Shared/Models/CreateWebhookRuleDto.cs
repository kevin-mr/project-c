namespace ProjectC.Shared.Models
{
    public class CreateWebhookRuleDto
    {
        public CreateWebhookRuleDto()
        {
            Method = WebhookRuleMethodDto.POST;
            Path = string.Empty;
            Description = string.Empty;
            RedirectUrl = string.Empty;
        }

        public WebhookRuleMethodDto Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
