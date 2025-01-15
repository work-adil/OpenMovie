using FluentValidation;
using OpenMovie.Application.CQRS.Models.Queries;

namespace OpenMovie.Application.CQRS.Handlers.Validators
{
    public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
    {
        public GetMoviesQueryValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Movie title is required.")
                .Length(3, 100).WithMessage("Movie title must be between 3 and 100 characters.");
            RuleFor(x => x.ApiKey)
                .NotEmpty().WithMessage("API Key is required.");

        }
    }
}
