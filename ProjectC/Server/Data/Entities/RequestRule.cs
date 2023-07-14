namespace ProjectC.Server.Data.Entities
{
    public class RequestRule
    {
        public RequestRule()
        {
            Id = 0;
            Method = RequestRuleMethod.GET;
            Path = string.Empty;
            PathRegex = string.Empty;
            Description = string.Empty;
            ResponseStatus = 200;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            ResponseDelay = 0;
            WorkflowActions = new List<WorkflowAction>();
            RequestRuleEvents = new List<RequestEvent>();
        }

        public int Id { get; set; }
        public RequestRuleMethod Method { get; set; }
        public string Path { get; set; }
        public string PathRegex { get; set; }
        public string Description { get; set; }
        public int ResponseStatus { get; set; }
        public int ResponseDelay { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }

        public IEnumerable<WorkflowAction> WorkflowActions { get; set; }
        public IEnumerable<RequestEvent> RequestRuleEvents { get; set; }
    }
}
