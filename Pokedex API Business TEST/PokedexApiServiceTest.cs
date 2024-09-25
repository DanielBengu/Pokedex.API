using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pokedex.API.Controllers.V1;
using Pokedex.API.Business.V1;
using Pokedex_API_Model.V1.Http;
using System.Threading.Tasks;
using Pokedex.API.Model.V1;
using static Pokedex_API_Model.V1.Http.HttpRequestGet;
using Microsoft.Extensions.Logging;

namespace Pokedex.API.Business.TEST
{
    [TestClass]
    public class PokemonControllerTests
    {
        private Mock<IPokedexApiService> _mockPokedexApiService;
        private Mock<ILogger<PokemonController>> _mockLogger;
        private PokemonController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockPokedexApiService = new Mock<IPokedexApiService>();
            _mockLogger = new Mock<ILogger<PokemonController>>();
            _controller = new PokemonController(_mockLogger.Object, _mockPokedexApiService.Object);
        }

        [TestMethod]
        public async Task GetBasicById_ReturnsOkWithPokemon()
        {
            string pokemonName = "mewtwo";
            _mockPokedexApiService.Setup(s => s.GetBasicById(It.IsAny<GetBasicRequest>()))
                .ReturnsAsync(new GetBasicResponse
                {
                    Pokemon = new Pokemon { Name = pokemonName },
                    Result = System.Net.HttpStatusCode.OK
                });

            var result = await _controller.GetBasicById(pokemonName);
            var okResult = result.Result as ObjectResult;

            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.AreEqual(pokemonName, (okResult?.Value as GetBasicResponse)?.Pokemon.Name);
        }

        [TestMethod]
        public async Task GetBasicById_ReturnsNotFoundForInvalidPokemon()
        {
            string invalidPokemon = "invalid";
            _mockPokedexApiService.Setup(s => s.GetBasicById(It.IsAny<GetBasicRequest>()))
                .ReturnsAsync(new GetBasicResponse
                {
                    Result = System.Net.HttpStatusCode.NotFound
                });

            var result = await _controller.GetBasicById(invalidPokemon);
            var notFoundResult = result.Result as ObjectResult;

            Assert.AreEqual(404, notFoundResult?.StatusCode);
        }
    }
}