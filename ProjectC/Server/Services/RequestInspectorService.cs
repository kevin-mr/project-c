using JsonFormatterPlus;
using ProjectC.Shared.Models;
using System.Text;

namespace ProjectC.Server.Services
{
    public class RequestInspectorService : IRequestInspectorService
    {
        private int _index = 0;

        public RequestInspectorService() { }

        public async Task<RequestDto> BuildRequestAsync(HttpRequest httpRequest)
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(httpRequest.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            var request = new RequestDto
            {
                Id = _index++,
                Method = httpRequest.Method,
                Headers = new List<RequestHeaderDto>
                {
                    new RequestHeaderDto
                    {
                        Name = "Content-Length",
                        Value = httpRequest.Headers.ContentLength.HasValue
                            ? httpRequest.Headers.ContentLength.Value.ToString()
                            : string.Empty
                    },
                    new RequestHeaderDto
                    {
                        Name = "Content-Type",
                        Value = httpRequest.Headers.ContentType.ToString()
                    }
                },
                Body = JsonFormatter.Format(stringBuilder.ToString()),
                ArrivalDate = DateTime.Now,
            };

            return request;
        }
    }
}
