using Pokedex_API_Data.V1.EndpointsClasses;

namespace Pokedex.API.Model.V1
{
    public class Pokemon
    {
        public string Name { get; set; } = string.Empty;

        public Habitat? Habitat { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool Legendary { get; set; }
    }
}