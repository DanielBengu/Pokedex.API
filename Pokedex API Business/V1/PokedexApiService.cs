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
                throw new ArgumentException("Error during GetBasicById request validation: {err}", errorMessage);

            return await _repository.GetBasicById(request.Pokemon);
        }

        public async Task<GetTranslatedResponse> GetTranslatedById(GetTranslatedRequest request)
        {
            if (!ValidateRequest(request, out string errorMessage))
                throw new ArgumentException("Error during GetTranslatedById request validation: {err}", errorMessage);

            return await _repository.GetTranslatedById(request.Pokemon); ;
        }

        bool ValidateRequest(BaseRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;

            //Add necessary request validation here

            return true;
        }

    }
}
