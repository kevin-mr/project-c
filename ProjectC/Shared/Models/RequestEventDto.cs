namespace ProjectC.Shared.Models
{
    public class RequestEventDto
    {
        public RequestEventDto()
        {
            Method = string.Empty;
            Headers = new Dictionary<string, string>();
            JsonHeaders = string.Empty;
            Body = string.Empty;
            JsonBody = string.Empty;
            ArrivalDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Method { get; set; }
        public string JsonHeaders { get; set; }
        public string JsonBody { get; set; }
        public DateTime ArrivalDate { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public int ArrivedSecondsAge => (DateTime.Now - ArrivalDate).Seconds;
    }
}
