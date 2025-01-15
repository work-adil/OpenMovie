using FluentAssertions;
using Moq;
using OpenMovie.Application.Services;
using OpenMovie.Core.Entities;
using OpenMovie.Infrastructure.Interfaces;

namespace OpenMovie.Application.ServicesTests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieApiClient> _mockMovieApiClient;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            // Arrange: Create the mock and the service instance
            _mockMovieApiClient = new Mock<IMovieApiClient>();
            _movieService = new MovieService(_mockMovieApiClient.Object);
        }

        [Fact]
        public async Task GetMoviesByTitleAsync_ShouldReturnMovies_WhenApiClientReturnsMovies()
        {
            // Arrange
            var title = "Inception";
            var apiKey = "validApiKey";
            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Year = "2010", Poster = "Inception posetr.jpg" },
                new Movie { Title = "Inception 2", Year = "2025", Poster = "Inception 2 posetr.jpg" }
            };

            // Mock the FetchMovieByTitleAsync method to return the expected movies
            _mockMovieApiClient.Setup(client => client.FetchMovieByTitleAsync(title, apiKey))
                               .ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetMoviesByTitleAsync(title, apiKey);

            // Assert
            result.Should().BeEquivalentTo(movies);
        }

        [Fact]
        public async Task GetMoviesByTitleAsync_ShouldReturnEmptyList_WhenApiClientReturnsNoMovies()
        {
            // Arrange
            var title = "NonExistentMovie";
            var apiKey = "validApiKey";
            var movies = new List<Movie>();

            // Mock the FetchMovieByTitleAsync method to return an empty list
            _mockMovieApiClient.Setup(client => client.FetchMovieByTitleAsync(title, apiKey))
                               .ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetMoviesByTitleAsync(title, apiKey);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMoviesByTitleAsync_ShouldThrowException_WhenApiClientThrowsException()
        {
            // Arrange
            var title = "Inception";
            var apiKey = "validApiKey";

            // Mock the FetchMovieByTitleAsync method to throw an exception
            _mockMovieApiClient.Setup(client => client.FetchMovieByTitleAsync(title, apiKey))
                               .ThrowsAsync(new System.Exception("API error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _movieService.GetMoviesByTitleAsync(title, apiKey));
        }
    }
}
