namespace ProjectC.Shared.Models
{
    public class CreateWorkflowStorageDto
    {
        public CreateWorkflowStorageDto()
        {
            PropertyIdentifier = string.Empty;
            Data = string.Empty;
        }

        public string PropertyIdentifier { get; set; }
        public string Data { get; set; }

        public int WorkflowId { get; set; }
    }
}
