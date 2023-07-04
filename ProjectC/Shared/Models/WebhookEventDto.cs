namespace ProjectC.Shared.Models
{
    public class WebhookEventDto
    {
        public WebhookEventDto()
        {
            Scheme = string.Empty;
            Host = string.Empty;
            Port = 0;
            PathBase = string.Empty;
            Path = string.Empty;
            QueryString = string.Empty;
            Method = string.Empty;
            Headers = new Dictionary<string, string?[]>();
            ContentType = string.Empty;
            Body = string.Empty;
            RedirectUrl = string.Empty;
        }

        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string? PathBase { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string?[]> Headers { get; set; }
        public string? ContentType { get; set; }
        public string Body { get; set; }
        public string RedirectUrl { get; set; }
    }
}
