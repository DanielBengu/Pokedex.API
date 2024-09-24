using System.Net;

namespace Pokedex_API_Data.V1
{
    public class ExternalHttpResponse
    {
        public object? Content { get; set; } = null;
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; } = null;
    }
}
