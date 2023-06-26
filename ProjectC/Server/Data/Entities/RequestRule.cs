namespace ProjectC.Server.Data.Entities
{
    public class RequestRule
    {
        public RequestRule()
        {
            Id = 0;
            Method = RequestRuleMethod.GET;
            Path = string.Empty;
            Description = string.Empty;
            ResponseMethod = RequestRuleMethod.GET;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            ResponseDelay = 0;
        }

        public int Id { get; set; }
        public RequestRuleMethod Method { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }

        public RequestRuleMethod ResponseMethod { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public int ResponseDelay { get; set; }
    }
}
