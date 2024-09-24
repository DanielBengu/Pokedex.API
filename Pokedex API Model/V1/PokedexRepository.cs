using static Pokedex_API_Model.V1.Http.HttpRequestGet;
using Microsoft.Extensions.Options;
using Pokedex.API.Model.V1;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pokedex_API_Data.V1.EndpointsClasses;
using System.Diagnostics;
using System.Net;

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
            try
            {
                var pokemon = await GetPokemonFromAPI(id);
                return new()
                {
                    Result = HttpStatusCode.OK,
                    Pokemon = pokemon,
                    Message = string.Empty
                };
            }
            catch (ArgumentException agEx)
            {
                return new()
                {
                    Result = HttpStatusCode.NotFound,
                    Message = agEx.Message,
                    Pokemon = null
                };
            }
            catch(Exception ex)
            {
                return new()
                {
                    Result = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Pokemon = null
                };
            }
        }

        public async Task<GetTranslatedResponse> GetTranslatedById(string id)
        {
            try
            {
                var pokemon = await GetPokemonFromAPI(id);
                FunTranslationsType translationType = GetTranslationType(pokemon);
                pokemon.Description = await CleanAndTranslateDescription(pokemon.Description, translationType);
                return new()
                {
                    Result = HttpStatusCode.OK,
                    Message = string.Empty,
                    Pokemon = pokemon
                };
            }
            catch (ArgumentException agEx)
            {
                return new()
                {
                    Result = HttpStatusCode.NotFound,
                    Message = agEx.Message,
                    Pokemon = null
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Result = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Pokemon = null
                };
            }
        }

        public async Task<T> SendHttpGetRequest<T>(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException($"{typeof(T)} Not Found Error: {response.ReasonPhrase}");
            else if (!response.IsSuccessStatusCode)
                throw new Exception($"{typeof(T)} Generic Error: {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content) ?? throw new Exception($"{typeof(T)} Serialization Error");
        }

        static FunTranslationsType GetTranslationType(Pokemon pokemon)
        {
            if (pokemon == null || pokemon.Description == null || pokemon.Description == string.Empty || pokemon.Habitat == null)
                return FunTranslationsType.Default;


            if (pokemon.Habitat.Name == POKEMON_CAVE_HABITAT || pokemon.Legendary)
                return FunTranslationsType.Yoda;
            else
                return FunTranslationsType.Shakespeare;
        }

        public async Task<Pokemon> GetPokemonFromAPI(string id, FunTranslationsType funTranslationType = FunTranslationsType.Default)
        {
            if (id == null || id.Trim() == string.Empty)
                throw new ArgumentException("Empty pokemon id");

            string pokeAPIEndpoint = $"{_endpoints.PokeAPI}/{id.ToLower()}";
            var pokeAPIResponse = await SendHttpGetRequest<PokeAPI>(pokeAPIEndpoint);
            var description = await CleanAndTranslateDescription(pokeAPIResponse.FlavorTextEntries.First().FlavorText, funTranslationType);
            return new()
            {
                Name = pokeAPIResponse.Name,
                Description = description,
                Habitat = pokeAPIResponse.Habitat,
                Legendary = pokeAPIResponse.IsLegendary
            };
        }

        public async Task<string> CleanAndTranslateDescription(string description, FunTranslationsType translationType)
        {
            var newDescription = description.Replace("\n", " ");

            if (translationType == FunTranslationsType.Default)
                return newDescription;

            return await GetTranslatedDescription(newDescription, translationType);
        }

        async Task<string> GetTranslatedDescription(string description, FunTranslationsType translationsType)
        {
            if (translationsType == FunTranslationsType.Default)
                return description;

            string funTranslationsEndPoint = $"{_endpoints.FunTranslations}/{translationsType}.json?text={description.Replace(" ", "%20")}";
            var funTranslations = await SendHttpGetRequest<FunTranslations>(funTranslationsEndPoint);
            return funTranslations.Contents.Translated;
        }

        public enum FunTranslationsType
        {
            Default,
            Yoda,
            Shakespeare
        }
    }
}
