using FluentValidation;
using Library.Core.Contracts.Author;
using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.Book
{
    public class RequestBookDtoValidator : AbstractValidator<RequestBookDto>
    {
        public RequestBookDtoValidator() {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(13).WithMessage("ISBN length should be 13 characters");

            RuleFor(x => x.GenreId)
                .NotEmpty().WithMessage("GenreId is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");

            RuleFor(x => x.AuthorId)
                 .NotEmpty().WithMessage("AuthorId is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");
        }
    }
}
