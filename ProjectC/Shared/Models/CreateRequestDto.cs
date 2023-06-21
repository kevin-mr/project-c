namespace ProjectC.Shared.Models
{
    public class CreateRequestDto
    {
        public CreateRequestDto()
        {
            Path = string.Empty;
            Method = RequestMethod.GET;
            Body = string.Empty;
        }

        public RequestMethod Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
    }
}
