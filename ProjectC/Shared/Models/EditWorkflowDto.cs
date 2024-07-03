namespace ProjectC.Shared.Models
{
    public class EditWorkflowDto
    {
        public EditWorkflowDto()
        {
            Id = 0;
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
