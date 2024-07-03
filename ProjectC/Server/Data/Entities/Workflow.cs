namespace ProjectC.Server.Data.Entities
{
    public class Workflow
    {
        public Workflow()
        {
            Id = 0;
            Name = string.Empty;
            RequestRuleVariants = new List<RequestRuleVariant>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<RequestRuleVariant> RequestRuleVariants { get; set; }
        public WorkflowStorage? WorkflowStorage { get; set; }
    }
}
