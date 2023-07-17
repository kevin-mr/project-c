namespace ProjectC.Shared.Models
{
    public class EditWorkflowTriggerDto
    {
        public EditWorkflowTriggerDto()
        {
            Id = 0;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public int WebhookEventId { get; set; }
    }
}
