using OpenMovie.Core.Entities;

namespace OpenMovie.Core.Interfaces
{
    public interface IMovieService
    {
        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">Title of the movie</param>
        /// <param name="apiKey">API Key</param>
        /// <returns>Movies</returns>
        Task<IEnumerable<Movie>> GetMoviesByTitleAsync(string title, string apiKey);
    }
}
