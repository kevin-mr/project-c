namespace ProjectC.Shared.Models
{
    public interface IRequestEventDto
    {
        string JsonHeaders { get; set; }
        string JsonBody { get; set; }
        Dictionary<string, string> Headers { get; set; }
        string Body { get; set; }
    }
}
