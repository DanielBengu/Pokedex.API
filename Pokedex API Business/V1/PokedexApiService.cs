using Pokedex_API_Data.V1;
using static Pokedex_API_Model.V1.Http.HttpRequestGet;

namespace Pokedex.API.Business.V1
{
    public interface IPokedexApiService
    {
        Task<GetBasicResponse> GetBasicById(GetBasicRequest pokemon);
        Task<GetTranslatedResponse> GetTranslatedById(GetTranslatedRequest pokemon);
    }

    public class PokedexApiService(IPokedexRepository repository): IPokedexApiService
    {
        readonly IPokedexRepository _repository = repository;

        public async Task<GetBasicResponse> GetBasicById(GetBasicRequest request)
        {
            if (!ValidateRequest(request, out string errorMessage))
                return new()
                {
                    Pokemon = null,
                    Message = $"Error during GetBasicById request validation: {errorMessage}",
                    Result = System.Net.HttpStatusCode.BadRequest
                };

            return await _repository.GetBasicById(request.Pokemon);
        }

        public async Task<GetTranslatedResponse> GetTranslatedById(GetTranslatedRequest request)
        {
            if (!ValidateRequest(request, out string errorMessage))
                return new()
                {
                    Pokemon = null,
                    Message = $"Error during GetBasicById request validation: {errorMessage}",
                    Result = System.Net.HttpStatusCode.BadRequest
                };

            return await _repository.GetTranslatedById(request.Pokemon); ;
        }

        bool ValidateRequest(BaseRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;

            //Add necessary  generic request validation here

            return true;
        }

    }
}
