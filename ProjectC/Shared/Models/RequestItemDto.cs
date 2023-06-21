namespace ProjectC.Shared.Models
{
    public class RequestItemDto
    {
        public int Id { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime ArrivalDate { get; set; }
    }
}
