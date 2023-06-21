using JsonFormatterPlus;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;
using System.Text;

namespace ProjectC.Server.Services
{
    public class RequestInspectorService : IRequestInspectorService
    {
        private int index = 0;

        public RequestInspectorService() { }

        public async Task<RequestDto> BuildRequestAsync(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            var requestDto = new RequestDto
            {
                Id = index++,
                Method = request.Method,
                Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
                ArrivalDate = DateTime.Now,
            };

            try
            {
                //requestDto.JsonBody = JsonFormatter.Format(stringBuilder.ToString());
                requestDto.JsonBody = stringBuilder.ToString();
            }
            catch (Exception)
            {
                requestDto.JsonBody = stringBuilder.ToString();
            }

            return requestDto;
        }
    }
}
