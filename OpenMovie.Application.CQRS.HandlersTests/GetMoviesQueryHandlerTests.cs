using FluentAssertions;
using Mapster;
using Moq;
using OpenMovie.Application.CQRS.Handlers.Handlers;
using OpenMovie.Application.CQRS.Models.Queries;
using OpenMovie.Application.CQRS.Models.Responses;
using OpenMovie.Core.Entities;
using OpenMovie.Core.Interfaces;

namespace OpenMovie.Application.CQRS.HandlersTests
{
    public class GetMoviesQueryHandlerTests
    {
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly GetMoviesQueryHandler _handler;

        public GetMoviesQueryHandlerTests()
        {
            // Arrange: Create the mock service and the query handler
            _mockMovieService = new Mock<IMovieService>();
            _handler = new GetMoviesQueryHandler(_mockMovieService.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovies_WhenServiceReturnsMovies()
        {
            // Arrange
            var query = new GetMoviesQuery { Title = "Inception", ApiKey = "validApiKey" };
            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Year = "2010", Poster = "Inception posetr.jpg" },
                new Movie { Title = "Inception 2", Year = "2025", Poster = "Inception 2 posetr.jpg" }
            };

            var movieResponses = movies.Adapt<IEnumerable<MovieResponse>>(); // Use Mapster's Adapt method

            // Mock the service to return the movies
            _mockMovieService.Setup(service => service.GetMoviesByTitleAsync(query.Title, query.ApiKey))
                             .ReturnsAsync(movies);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeEquivalentTo(movieResponses);
            result.IsSuccess.Should().BeTrue(); // Assuming success result for this case
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenServiceReturnsNoMovies()
        {
            // Arrange
            var query = new GetMoviesQuery { Title = "NonExistentMovie", ApiKey = "validApiKey" };
            var movies = new List<Movie>(); // Empty list of movies
            var movieResponses = movies.Adapt<IEnumerable<MovieResponse>>(); // Map to empty response list

            // Mock the service to return an empty list of movies
            _mockMovieService.Setup(service => service.GetMoviesByTitleAsync(query.Title, query.ApiKey))
                             .ReturnsAsync(movies);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeEmpty();
            result.IsSuccess.Should().BeTrue(); // Assuming success result even if no movies are found
        }
    }
}
