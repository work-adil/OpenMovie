namespace OpenMovie.Application.CQRS.Models.Responses
{
    /// <param name="Title"> Title of the movie </param>
    /// <param name="Year"> Released year </param>
    /// <param name="Poster"> Url pointing to the poster. </param>
    public record MovieResponse(string Title, string Year, string Poster);
}
