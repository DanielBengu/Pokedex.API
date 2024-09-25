using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Business.V1;
using static Pokedex_API_Model.V1.Http.HttpRequestGet;

namespace Pokedex.API.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController(ILogger<PokemonController> logger, IPokedexApiService apiService) : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger = logger;
        private readonly IPokedexApiService _apiService = apiService;

        [HttpGet("{pokemonId}")]
        public async Task<ActionResult<GetBasicResponse>> GetBasicById(string pokemonId)
        {
            _logger.LogInformation("GetBasicById request for Pokemon: {pokemonId}", pokemonId);

            var request = new GetBasicRequest { Pokemon = pokemonId };
            var response = await _apiService.GetBasicById(request);

            return StatusCode((int)response.Result, response);
        }

        [HttpGet("Translated/{pokemonId}")]
        public async Task<ActionResult<GetTranslatedResponse>> GetTranslatedById(string pokemonId)
        {
            _logger.LogInformation("GetTranslatedById request for Pokemon: {pokemonId}", pokemonId);

            var request = new GetTranslatedRequest { Pokemon = pokemonId };
            var response = await _apiService.GetTranslatedById(request);

            return StatusCode((int)response.Result, response);
        }
    }
}