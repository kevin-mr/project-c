namespace ProjectC.Server.Data.Entities
{
    public class Request
    {
        public Request()
        {
            Path = string.Empty;
            Body = string.Empty;
        }

        public int Id { get; set; }
        public int Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
    }
}
