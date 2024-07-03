namespace ProjectC.Shared.Models
{
    public class WorkflowDto
    {
        public WorkflowDto()
        {
            Id = 0;
            Name = string.Empty;
            RequestRuleVariants = new List<RequestRuleVariantDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<RequestRuleVariantDto> RequestRuleVariants { get; set; }
        public WorkflowStorageDto? WorkflowStorage { get; set; }
    }
}
