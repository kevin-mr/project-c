namespace ProjectC.Shared.Models
{
    public class RequestRuleDto
    {
        public RequestRuleDto()
        {
            Id = 0;
            Method = RequestRuleMethodDto.POST;
            Path = string.Empty;
            Description = string.Empty;
            ResponseStatus = 200;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            ResponseDelay = 0;
            WorkflowActions = new List<WorkflowActionDto>();
            RequestRuleEvents = new List<RequestEventDto>();
        }

        public int Id { get; set; }
        public RequestRuleMethodDto Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int ResponseDelay { get; set; }
        public int ResponseStatus { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }

        public string MethodLabel => Method.ToString();

        public IEnumerable<WorkflowActionDto> WorkflowActions { get; set; }
        public IEnumerable<RequestEventDto> RequestRuleEvents { get; set; }
    }
}
