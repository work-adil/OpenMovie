namespace OpenMovie.Application.CQRS.Models.Responses
{
    /// <summary>
    /// Base class for results
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class GenericBaseResult<TModel> : BaseResult
    {
        public GenericBaseResult(TModel? model)
        {
            Result = model;
        }

        public GenericBaseResult() : this(default) { }

        /// <summary>
        /// Result object.
        /// </summary>
        public TModel? Result { get; set; }
    }
}
