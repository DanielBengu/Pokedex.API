using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex_API_Data.V1.EndpointsClasses
{
    public class PokeAPI
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; } = default!;

        [JsonPropertyName("flavor_text_entries")]
        public IEnumerable<PokedexEntry> FlavorTextEntries { get; set; } = default!;
    }

    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class PokedexEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; } = string.Empty;
    }
}