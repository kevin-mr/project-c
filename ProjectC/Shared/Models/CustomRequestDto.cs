namespace ProjectC.Shared.Models
{
    public class CustomRequestDto
    {
        public CustomRequestDto()
        {
            Path = string.Empty;
            Body = string.Empty;
        }

        public int Id { get; set; }
        public int Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }

        public string MethodLabel => ((RequestMethod)Method).ToString();
    }
}
