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
        private readonly Endpoints _endpoints = endpoints.Value;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<GetBasicResponse> GetBasicById(string id)
        {
            string pokeAPIEndpoint = $"{_endpoints.PokeAPI}/{id}";
            var pokeAPI = await SendHttpGetRequest<PokeAPI>(pokeAPIEndpoint);
            return new()
            {
                Pokemon = new()
                {
                    Name = pokeAPI.Name,
                    Description = CleanDescription(pokeAPI.FlavorTextEntries.First().FlavorText),
                    Habitat = pokeAPI.Habitat.Name,
                    Legendary = pokeAPI.IsLegendary
                }
            };
        }

        public async Task<GetTranslatedResponse> GetTranslatedById(string id)
        {
            string pokeAPIEndpoint = $"{_endpoints.PokeAPI}/{id}";
            var pokeAPI = await SendHttpGetRequest<PokeAPI>(pokeAPIEndpoint);
            return new()
            {
                Pokemon = new()
                {
                    Name = pokeAPI.Name,
                    Description = CleanDescription(pokeAPI.FlavorTextEntries.First().FlavorText),
                    Habitat = pokeAPI.Habitat.Name,
                    Legendary = pokeAPI.IsLegendary
                }
            };
        }

        public async Task<T> SendHttpGetRequest<T>(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content) ?? throw new Exception("Error during data serialization from API"); ;
        }

        public static string CleanDescription(string description)
        {
            return description.Replace("\n", " ").Replace("\f", " ");
        }

        public enum HttpActions
        {
            Get,
            Post,
        }
    }
}
