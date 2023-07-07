namespace ProjectC.Server.Data.Entities
{
    public class Workflow
    {
        public Workflow()
        {
            Id = 0;
            Name = string.Empty;
            WorkflowActions = new List<WorkflowAction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<WorkflowAction> WorkflowActions { get; set; }
    }
}
