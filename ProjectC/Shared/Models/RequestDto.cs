namespace ProjectC.Shared.Models
{
    public class RequestDto
    {
        public string Method { get; set; }
        public IEnumerable<RequestHeaderDto> Headers { get; set; }
        public string Body { get; set; }
        public DateTime ArrivalDate { get; set; }

        public RequestDto()
        {
            Method = string.Empty;
            Headers = new List<RequestHeaderDto>();
            Body = string.Empty;
            ArrivalDate = DateTime.Now;
        }
    }
}
