namespace ProjectC.Shared.Models
{
    public class CreateWorkflowTriggerDto
    {
        public CreateWorkflowTriggerDto()
        {
            Description = string.Empty;
        }

        public string Description { get; set; }

        public int WorkflowActionId { get; set; }
        public int WebhookEventId { get; set; }
    }
}
