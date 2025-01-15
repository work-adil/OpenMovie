using OpenMovie.Core.Entities;

namespace OpenMovie.Infrastructure.Services.Models
{
    /// <param name="Search"> Search result. </param>
    public record OmdbMoviesResult(IEnumerable<Movie> Search);
}
