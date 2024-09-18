namespace Pokedex.API.Model.V1
{
    public class HttpRequestGet
    {
        public class GetBasicRequest
        {
            public string Pokemon { get; set; } = string.Empty;
        }

        public class GetBasicResponse
        {
            public Pokemon Pokemon { get; set; } = default!;
        }

        public class GetTranslatedRequest
        {
            public string Pokemon { get; set; } = string.Empty;
        }

        public class GetTranslatedResponse
        {
            public Pokemon Pokemon { get; set; } = default!;
        }
    }
}
