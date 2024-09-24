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

        [HttpGet]
        public async Task<ActionResult<GetBasicResponse>> GetBasicById([FromBody]GetBasicRequest pokemon)
        {
            _logger.LogInformation("GetBasicById request for Pokemon: {pokemon}", pokemon);

            var response = await _apiService.GetBasicById(pokemon);

            return StatusCode((int)response.Result, response);
        }

        [HttpGet("Translated")]
        public async Task<ActionResult<GetTranslatedResponse>> GetTranslatedById([FromBody]GetTranslatedRequest pokemon)
        {
            _logger.LogInformation("GetTranslatedById request for Pokemon: {pokemon}", pokemon);
            var response = await _apiService.GetTranslatedById(pokemon);
            
            return StatusCode((int)response.Result, response);
        }
    }
}