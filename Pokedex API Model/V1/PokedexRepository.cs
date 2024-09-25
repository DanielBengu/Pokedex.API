using static Pokedex_API_Model.V1.Http.HttpRequestGet;
using Microsoft.Extensions.Options;
using Pokedex.API.Model.V1;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pokedex_API_Data.V1.EndpointsClasses;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata.Ecma335;

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

                if(pokemon == null)
                    return new()
                    {
                        Result = HttpStatusCode.NotFound,
                        Message = $"Pokemon {id} not found",
                        Pokemon = null
                    };

                return new()
                {
                    Result = HttpStatusCode.OK,
                    Pokemon = pokemon,
                    Message = string.Empty
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

                if (pokemon == null)
                    return new()
                    {
                        Result = HttpStatusCode.NotFound,
                        Message = $"Pokemon {id} not found",
                        Pokemon = null
                    };

                FunTranslationsType translationType = GetTranslationType(pokemon);
                pokemon.Description = await CleanAndTranslateDescription(pokemon.Description, translationType);

                return new()
                {
                    Result = HttpStatusCode.OK,
                    Pokemon = pokemon,
                    Message = string.Empty
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

        public async Task<HttpResponseMessage> SendHttpGetRequest(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            return response;
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

        public async Task<Pokemon?> GetPokemonFromAPI(string id)
        {
            if (id == null || id.Trim() == string.Empty)
                return null;

            string pokeAPIEndpoint = $"{_endpoints.PokeAPI}/{id.ToLower()}";
            var pokeAPIResponse = await SendHttpGetRequest(pokeAPIEndpoint);

            if (!pokeAPIResponse.IsSuccessStatusCode)
                return null;

            var content = await pokeAPIResponse.Content.ReadAsStringAsync();
            var contentDeserialied = JsonSerializer.Deserialize<PokeAPI>(content);

            if (contentDeserialied == null)
                return null;

            return new()
            {
                Name = contentDeserialied.Name,
                Description = contentDeserialied.FlavorTextEntries.First().FlavorText,
                Habitat = contentDeserialied.Habitat,
                Legendary = contentDeserialied.IsLegendary
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
            var funTranslations = await SendHttpGetRequest(funTranslationsEndPoint);
            var content = await funTranslations.Content.ReadAsStringAsync();
            var contentDeserialied = JsonSerializer.Deserialize<FunTranslations>(content);

            if (contentDeserialied == null)
                return description;

            return contentDeserialied.Contents.Translated;
        }

        public enum FunTranslationsType
        {
            Default,
            Yoda,
            Shakespeare
        }
    }
}
