namespace ProjectC.Shared.Models
{
    public class EditWorkflowStorageDto
    {
        public EditWorkflowStorageDto()
        {
            Id = 0;
            PropertyIdentifier = string.Empty;
            Data = string.Empty;
        }

        public int Id { get; set; }
        public string PropertyIdentifier { get; set; }
        public string Data { get; set; }
    }
}
