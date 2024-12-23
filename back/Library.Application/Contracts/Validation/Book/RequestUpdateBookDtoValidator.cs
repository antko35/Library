﻿using FluentValidation;
using Library.Application.Contracts.Book;

namespace Library.Application.Contracts.Validation.Book
{
    public class RequestUpdateBookDtoValidator : AbstractValidator<RequestUpdateBookDto>
    {
        public RequestUpdateBookDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(17).WithMessage("ISBN length should be 17 characters");

            RuleFor(x => x.GenreId)
                .NotEmpty().WithMessage("GenreId is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");

            RuleFor(x => x.AuthorId)
                 .NotEmpty().WithMessage("AuthorId is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");
        }
    }
}
