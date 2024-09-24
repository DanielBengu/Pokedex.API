using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex_API_Data.V1.EndpointsClasses
{
    public class FunTranslations
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; } = default!;
    }

    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; } = string.Empty;
    }
}