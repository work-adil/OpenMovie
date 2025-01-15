namespace OpenMovie.Core.Entities
{
    public class Movie
    {
        /// <summary>
        /// Title of the movie
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Released year
        /// </summary>
        public required string Year { get; set; }

        /// <summary>
        /// Url pointing to the poster.
        /// </summary>
        public required string Poster { get; set; }
    }
}
