using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OpenMovie.Application.CQRS.Models.Queries;
using OpenMovie.Application.CQRS.Models.Responses;
using System;
using System.Net;

namespace OpenMovie.APITests
{
    public class MoviesModuleTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IMediator> _mediatorMock;

        public MoviesModuleTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _mediatorMock = new Mock<IMediator>();

            // Arrange mediatr mock
            var movieTitle = "Prince";
            var apiKey = "validApiKey";
            var movieResponse = new List<MovieResponse>
            {
                new MovieResponse("Prince", "2010", "https://example.com/Prince.jpg"),
                new MovieResponse("Prince of Persia", "2012", "https://example.com/pop.jpg")
            };
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetMoviesQuery>(q => q.Title == movieTitle && q.ApiKey == apiKey), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GenericBaseResult<IEnumerable<MovieResponse>>(movieResponse));


            _mediatorMock
                .Setup(m => m.Send(It.Is<GetMoviesQuery>(q => q.Title != movieTitle || q.ApiKey != apiKey), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GenericBaseResult<IEnumerable<MovieResponse>>(Enumerable.Empty<MovieResponse>()));

        }

        private void SetupMockForMovieQuery(string title, string apiKey, List<MovieResponse> response)
        {
            
        }

        [Fact]
        public async Task GetMovie_ReturnsOk_WhenMoviesExist()
        {
            // Arrange            
            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mediatorMock.Object);
                });
            }).CreateClient();

            var response = await client.GetAsync($"api/movies?s=Prince&apikey=validApiKey");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Prince");  // Using FluentAssertions
        }

        [Fact]
        public async Task GetMovie_ReturnsNotFound_WhenMoviesDoNotExist()
        {
            // Arrange            
            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mediatorMock.Object);
                });
            }).CreateClient();
            var response = await client.GetAsync($"api/movies?s=somemovie&apikey=validApiKey");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // Using FluentAssertions
        }

        [Fact]
        public async Task GetMovie_ReturnsNotFound_WhenKeyIsInvalid()
        {
            // Arrange            
            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mediatorMock.Object);
                });
            }).CreateClient();
            var response = await client.GetAsync($"api/movies?s=Prince&apikey=inValidApiKey");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // Using FluentAssertions
        }
    }
}
