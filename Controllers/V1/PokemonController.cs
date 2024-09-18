using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Business.V1;
using static Pokedex.API.Model.V1.HttpRequestGet;

namespace Pokedex_API.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController(ILogger<PokemonController> logger, IPokedexApiService apiService) : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger = logger;
        private readonly IPokedexApiService _apiService = apiService;

        [HttpGet]
        public GetBasicResponse GetBasicById([FromBody] GetBasicRequest pokemon)
        {
            _logger.LogInformation("GetBasicById request for Pokemon: {pokemon}", pokemon);
            return _apiService.GetBasicById(pokemon);
        }

        [HttpGet("Translated")]
        public GetTranslatedResponse GetTranslatedById([FromBody] GetTranslatedRequest pokemon)
        {
            _logger.LogInformation("GetTranslatedById request for Pokemon: {pokemon}", pokemon);
            return _apiService.GetTranslatedById(pokemon);
        }
    }
}
