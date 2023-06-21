namespace ProjectC.Shared.Models
{
    public class CreateRequestRuleDto
    {
        public CreateRequestRuleDto()
        {
            Method = RequestRuleMethodDto.GET;
            Path = string.Empty;
            Description = string.Empty;
            ResponseMethod = RequestRuleMethodDto.GET;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            ResponseDelay = 0;
        }

        public RequestRuleMethodDto Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public RequestRuleMethodDto ResponseMethod { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public int ResponseDelay { get; set; }
    }
}
