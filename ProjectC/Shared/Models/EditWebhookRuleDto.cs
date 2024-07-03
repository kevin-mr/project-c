namespace ProjectC.Shared.Models
{
    public class EditWebhookRuleDto
    {
        public EditWebhookRuleDto()
        {
            Id = 0;
            Method = WebhookRuleMethodDto.POST;
            Path = string.Empty;
            Description = string.Empty;
            RedirectUrl = string.Empty;
        }

        public int Id { get; set; }
        public WebhookRuleMethodDto Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
