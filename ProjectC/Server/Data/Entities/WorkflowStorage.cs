namespace ProjectC.Server.Data.Entities
{
    public class WorkflowStorage
    {
        public WorkflowStorage()
        {
            Id = 0;
            PropertyIdentifier = string.Empty;
            Data = string.Empty;
        }

        public int Id { get; set; }
        public string PropertyIdentifier { get; set; }
        public string Data { get; set; }

        public int WorkflowId { get; set; }
    }
}
