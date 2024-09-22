using static Pokedex_API_Model.V1.Http.HttpRequestGet;
using Microsoft.Extensions.Options;
using Pokedex.API.Model.V1;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pokedex_API_Data.V1.EndpointsClasses;

namespace Pokedex_API_Data.V1
{
    public interface IPokedexRepository
    {
        Task<GetBasicResponse> GetBasicById(string id);
        Task<GetTranslatedResponse> GetTranslatedById(string id);
    }

    public class PokedexRepository(IOptions<Endpoints> endpoints, HttpClient httpClient) : IPokedexRepository
    {
        private const string POKEMON_CAVE_HABITAT = "cave";
        private readonly Endpoints _endpoints = endpoints.Value;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<GetBasicResponse> GetBasicById(string id)
        {
            var pokemon = await GetPokemonFromAPI(id);
            return new()
            {
                Pokemon = pokemon
            };
        }

        public async Task<GetTranslatedResponse> GetTranslatedById(string id)
        {
            var pokemon = await GetPokemonFromAPI(id);
            FunTranslationsType translationType = GetTranslationType(pokemon);
            var translatedDescription = 
            return new()
            {
                Pokemon = pokemon
            };
        }

        public async Task<T> SendHttpGetRequest<T>(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content) ?? throw new Exception("Error during data serialization from API"); ;
        }

        static FunTranslationsType GetTranslationType(Pokemon pokemon)
        {
            if (pokemon == null || pokemon.Description == null || pokemon.Description == string.Empty)
                return FunTranslationsType.Default;


            if (pokemon.Habitat == POKEMON_CAVE_HABITAT || pokemon.Legendary)
                return FunTranslationsType.Yoda;
            else
                return FunTranslationsType.Shakespeare;
        }

        public async Task<Pokemon> GetPokemonFromAPI(string id, FunTranslationsType funTranslationType = FunTranslationsType.Default)
        {
            string pokeAPIEndpoint = $"{_endpoints.PokeAPI}/{id}";
            var pokeAPI = await SendHttpGetRequest<PokeAPI>(pokeAPIEndpoint);

            var description = CleanAndTranslateDescription(pokeAPI.FlavorTextEntries.First().FlavorText, funTranslationType);
            return new()
            {
                Name = pokeAPI.Name,
                Description = description,
                Habitat = pokeAPI.Habitat.Name,
                Legendary = pokeAPI.IsLegendary
            };
        }

        public static string CleanAndTranslateDescription(string description, FunTranslationsType translationType)
        {
            return description.Replace("\n", " ").Replace("\f", " ");
        }

        public enum FunTranslationsType
        {
            Default,
            Yoda,
            Shakespeare
        }
    }
}
