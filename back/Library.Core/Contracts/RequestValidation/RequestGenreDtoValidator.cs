using FluentValidation;
using Library.Core.Contracts.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.Genre
{
    public class RequestGenreDtoValidator : AbstractValidator<RequestGenreDto>
    {
        public RequestGenreDtoValidator()
        {
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");
        }
        public string Genre { get; set; } = string.Empty;
    }
}
