using FluentValidation;
using Library.Application.Contracts.Book;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Validation.Book
{
    public class RequestUploadCoverValidator : AbstractValidator<RequestUploadCoverDto>
    {
        public RequestUploadCoverValidator()
        {
            RuleFor(x => x.BookId)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");
            RuleFor(x => x.File)
                .NotEmpty();

            
        }
    }
}
