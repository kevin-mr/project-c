namespace ProjectC.Server.Data.Entities
{
    public class WorkflowTrigger
    {
        public WorkflowTrigger()
        {
            Id = 0;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public int WorkflowActionId { get; set; }
        public WorkflowAction? WorkflowAction { get; set; }
        public int WebhookEventId { get; set; }
        public WebhookEvent? WebhookEvent { get; set; }
    }
}
