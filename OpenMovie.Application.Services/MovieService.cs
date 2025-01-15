using OpenMovie.Core.Entities;
using OpenMovie.Core.Interfaces;
using OpenMovie.Infrastructure.Interfaces;

namespace OpenMovie.Application.Services
{
    public class MovieService (IMovieApiClient movieApiClient) : IMovieService
    {
        public Task<IEnumerable<Movie>> GetMoviesByTitleAsync(string title, string apiKey)
            => movieApiClient.FetchMovieByTitleAsync(title, apiKey);        
    }
}
