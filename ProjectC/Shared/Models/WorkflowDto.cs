namespace ProjectC.Shared.Models
{
    public class WorkflowDto
    {
        public WorkflowDto()
        {
            Id = 0;
            Name = string.Empty;
            WorkflowActions = new List<WorkflowActionDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<WorkflowActionDto> WorkflowActions { get; set; }
        public WorkflowStorageDto? WorkflowStorage { get; set; }
    }
}
