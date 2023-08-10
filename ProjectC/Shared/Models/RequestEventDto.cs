using System.Drawing;

namespace ProjectC.Shared.Models
{
    public class RequestEventDto : IRequestEventDto
    {
        public RequestEventDto()
        {
            Id = 0;
            Method = string.Empty;
            Headers = new Dictionary<string, string>();
            JsonHeaders = string.Empty;
            Body = string.Empty;
            JsonBody = string.Empty;
            ArrivalDate = DateTime.Now;
            Path = string.Empty;
            WorkflowActionDescription = string.Empty;
        }

        public int Id { get; set; }
        public string Method { get; set; }
        public string JsonHeaders { get; set; }
        public string JsonBody { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Path { get; set; }
        public string? RedirectUrl { get; set; }
        public string WorkflowActionDescription { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public int? WorkflowActionId { get; set; }
        public int? RequestRuleId { get; set; }
        public int? WebhookRuleId { get; set; }

        public string ArrivedTimeLabel()
        {
            var totalSeconds = (int)Math.Floor((DateTime.UtcNow - ArrivalDate).TotalSeconds);
            if (totalSeconds < 60)
            {
                return $"{totalSeconds} seconds ago";
            }
            else if (totalSeconds < 3600)
            {
                return $"{totalSeconds / 60} minutes ago";
            }
            else if (totalSeconds < 86400)
            {
                return $"{totalSeconds / 3600} minutes ago";
            }
            else
            {
                return "long time ago";
            }
        }
    }
}
