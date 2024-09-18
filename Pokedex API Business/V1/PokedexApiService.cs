using static Pokedex.API.Model.V1.HttpRequestGet;

namespace Pokedex.API.Business.V1
{
    public interface IPokedexApiService
    {
        GetBasicResponse GetBasicById(GetBasicRequest pokemon);
        GetTranslatedResponse GetTranslatedById(GetTranslatedRequest pokemon);
    }

    public class PokedexApiService : IPokedexApiService
    {
        public GetBasicResponse GetBasicById(GetBasicRequest request)
        {
            return new()
            {
                Pokemon = new()
                {
                    Name = request.Pokemon,
                    Description = request.Pokemon,
                    Habitat = request.Pokemon,
                    IsLegendary = true
                }
            };
        }

        public GetTranslatedResponse GetTranslatedById(GetTranslatedRequest request)
        {
            return new()
            {
                Pokemon = new()
                {
                    Name = request.Pokemon,
                    Description = request.Pokemon,
                    Habitat = request.Pokemon,
                    IsLegendary = true
                }
            };
        }

    }
}
