using Mapster;
using MediatR;
using OpenMovie.Application.CQRS.Models.Queries;
using OpenMovie.Application.CQRS.Models.Responses;
using OpenMovie.Core.Interfaces;

namespace OpenMovie.Application.CQRS.Handlers.Handlers
{
    /// <summary>
    /// Handler for GetMoviesQuery
    /// </summary>
    public class GetMoviesQueryHandler (IMovieService movieService) : IRequestHandler<GetMoviesQuery, GenericBaseResult<IEnumerable<MovieResponse>>>
    {        
        public async Task<GenericBaseResult<IEnumerable<MovieResponse>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
            => new GenericBaseResult<IEnumerable<MovieResponse>>((await movieService.GetMoviesByTitleAsync(request.Title, request.ApiKey)).Adapt<IEnumerable<MovieResponse>>());
    }
}
