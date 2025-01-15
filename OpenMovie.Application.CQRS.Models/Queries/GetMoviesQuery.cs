using MediatR;
using OpenMovie.Application.CQRS.Models.Responses;

namespace OpenMovie.Application.CQRS.Models.Queries
{
    /// <summary>
    /// Gets movies by title.
    /// </summary>
    public class GetMoviesQuery : IRequest<GenericBaseResult<IEnumerable<MovieResponse>>>
    {
        /// <summary>
        /// Title
        /// </summary>
        public required string Title { get; set; }
        
        /// <summary>
        /// API Key 
        /// </summary>
        public required string ApiKey { get; set; }
    }
}
