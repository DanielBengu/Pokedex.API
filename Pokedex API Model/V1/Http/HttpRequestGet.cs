using Pokedex.API.Model.V1;
using System.Net;

namespace Pokedex_API_Model.V1.Http
{
    public class HttpRequestGet
    {
        public class BaseRequest
        {
        }

        public class BaseResponse
        {
            public HttpStatusCode Result { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        public class GetBasicRequest : BaseRequest
        {
            public string Pokemon { get; set; } = string.Empty;
        }

        public class GetBasicResponse : BaseResponse
        {

            public Pokemon? Pokemon { get; set; } = default!;
        }

        public class GetTranslatedRequest : BaseRequest
        {
            public string Pokemon { get; set; } = string.Empty;
        }

        public class GetTranslatedResponse : BaseResponse
        {
            public Pokemon? Pokemon { get; set; } = default!;
        }
    }
}
