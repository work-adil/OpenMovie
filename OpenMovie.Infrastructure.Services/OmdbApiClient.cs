using Newtonsoft.Json;
using OpenMovie.Application.CQRS.Models.Responses;
using OpenMovie.Core.Entities;
using OpenMovie.Infrastructure.Interfaces;
using OpenMovie.Infrastructure.Services.Models;

namespace OpenMovie.Infrastructure.Services
{
    public class OmdbApiClient (HttpClient httpClient) : IMovieApiClient
    {
        public async Task<IEnumerable<Movie>> FetchMovieByTitleAsync(string title, string apiKey)
        {
            var requestUrl = $"http://www.omdbapi.com/?s={title}&apikey={apiKey}";
            try
            {
                var response = await httpClient.GetStringAsync(requestUrl);
                if (response == null || response.Contains("\"Response\":\"False\""))
                    return Enumerable.Empty<Movie>();

                var result = JsonConvert.DeserializeObject<OmdbMoviesResult>(response)!;
                return result.Search;
            }
            catch
            {
                throw new OpenMovieException(OpenMovieErrorCodes.DataFetchingError, "Unable to fetch data from the server");
            }
        }
    }
}
