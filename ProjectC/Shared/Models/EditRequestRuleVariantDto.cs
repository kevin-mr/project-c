namespace ProjectC.Shared.Models
{
    public class EditRequestRuleVariantDto
    {
        public EditRequestRuleVariantDto()
        {
            Id = 0;
            Description = string.Empty;
            ResponseStatus = 0;
            ResponseDelay = 0;
            ResponseHeaders = string.Empty;
            ResponseBody = string.Empty;
            RequestRuleId = 0;
        }

        public int Id { get; set; }
        public RequestRuleMethodDto? Method { get; set; }
        public string? Path { get; set; }
        public string Description { get; set; }
        public int? ResponseStatus { get; set; }
        public int? ResponseDelay { get; set; }
        public string? ResponseHeaders { get; set; }
        public string? ResponseBody { get; set; }

        public int? RequestRuleId { get; set; }
    }
}
