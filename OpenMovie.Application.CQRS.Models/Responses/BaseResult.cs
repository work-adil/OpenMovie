using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace OpenMovie.Application.CQRS.Models.Responses
{
    public class BaseResult
    {
        public static BaseResult Default() => new BaseResult();

        public static bool IsInDebuggingMode { get; set; }

        public bool IsSuccess
         => ResponseStatusCode == HttpStatusCode.OK || ResponseStatusCode == HttpStatusCode.Created || ResponseStatusCode == HttpStatusCode.Accepted;

        /// <summary>
        /// Message if any.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Errors if any.
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Errorcode.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;

        public void AddErrors(params string[] messages)
        {
            if (Errors == null) // Initialize if needed.
                Errors = new List<string>();

            Errors.AddRange(messages);
        }

        /// <summary>
        /// Adds exception to errors list in the response in debug mode.
        /// </summary>
        /// <param name="ex">Exception to add in error list.</param>
        public void AddExceptionLog(Exception ex)
        {
            if (ResponseStatusCode == HttpStatusCode.OK)
                ResponseStatusCode = HttpStatusCode.BadRequest;

            // It is really bad idea to show exceptions in production.
            if (IsInDebuggingMode || ex is ValidationException || ex is ArgumentNullException || ex is InvalidOperationException)
            {
                AddErrors(ex.Message);
                if (ex.InnerException != null)
                    AddExceptionLog(ex.InnerException);
            }

            if (ex is OpenMovieException bargainingPowerException)
            {
                ErrorCode = (int)bargainingPowerException.ErrorCode;
                AddErrors(ex.Message);
            }
        }
    }
}
