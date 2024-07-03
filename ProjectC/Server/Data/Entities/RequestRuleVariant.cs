namespace ProjectC.Server.Data.Entities
{
    public class RequestRuleVariant
    {
        public RequestRuleVariant()
        {
            Id = 0;
            Description = string.Empty;
            ResponseStatus = 200;
            ResponseDelay = 0;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            WorkflowId = 0;
            RequestRuleId = 0;
            RequestRuleVariantEvents = new List<RequestEvent>();
            RequestRuleTriggers = new List<RequestRuleTrigger>();
        }

        public int Id { get; set; }
        public RequestRuleMethod? Method { get; set; }
        public string? Path { get; set; }
        public string? PathRegex { get; set; }
        public string Description { get; set; }
        public int ResponseStatus { get; set; }
        public int ResponseDelay { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }

        public int? RequestRuleId { get; set; }
        public RequestRule? RequestRule { get; set; }

        public int WorkflowId { get; set; }
        public Workflow? Workflow { get; set; }
        public IEnumerable<RequestEvent> RequestRuleVariantEvents { get; set; }
        public IEnumerable<RequestRuleTrigger> RequestRuleTriggers { get; set; }
    }
}
