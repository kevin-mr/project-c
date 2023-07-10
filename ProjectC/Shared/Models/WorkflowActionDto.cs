namespace ProjectC.Shared.Models
{
    public class WorkflowActionDto
    {
        public WorkflowActionDto()
        {
            Id = 0;
            Name = string.Empty;
            ResponseStatus = 0;
            ResponseDelay = 0;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            WorkflowId = 0;
            RequestRuleId = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int? ResponseStatus { get; set; }
        public int? ResponseDelay { get; set; }
        public string? ResponseHeaders { get; set; }
        public string? ResponseBody { get; set; }

        public int WorkflowId { get; set; }

        public int RequestRuleId { get; set; }
        public RequestRuleDto? RequestRule { get; set; }
    }
}
