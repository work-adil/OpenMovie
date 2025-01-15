using OpenMovie.Core.Entities;

namespace OpenMovie.Infrastructure.Interfaces
{
    public interface IMovieApiClient
    {
        /// <summary>
        /// Fetch movies by title.
        /// </summary>
        /// <param name="title">Title of the movie.</param>
        /// <param name="apiKey">API Key.</param>
        /// <returns>Movies</returns>
        Task<IEnumerable<Movie>> FetchMovieByTitleAsync(string title, string apiKey);
    }
}
