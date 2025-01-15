namespace OpenMovie.Core.Common
{
    [Serializable]
    public class OpenMovieException : Exception
    {
        public OpenMovieErrorCodes ErrorCode { get; set; }
        public OpenMovieException(OpenMovieErrorCodes errorCode, string? message) : base(message)
            => ErrorCode = errorCode;
    }

    public enum OpenMovieErrorCodes
    {
        APIKeyMissing = 1,
        MovieNotFound = 2,        
    }
}