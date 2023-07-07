namespace ProjectC.Server.Data.Entities
{
    public class WorkflowAction
    {
        public WorkflowAction()
        {
            Id = 0;
            Name = string.Empty;
            ResponseStatus = 0;
            ResponseDelay = 0;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            WorkFlowId = 0;
            RequestRuleId = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ResponseStatus { get; set; }
        public int? ResponseDelay { get; set; }
        public string? ResponseHeaders { get; set; }
        public string? ResponseBody { get; set; }

        public int RequestRuleId { get; set; }
        public RequestRule? RequestRule { get; set; }

        public int WorkFlowId { get; set; }
        public Workflow? WorkFlow { get; set; }
    }
}
