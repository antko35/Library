using AutoMapper;
using FluentValidation;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.RequestValidation
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator() 
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                 .MaximumLength(20).WithMessage("UserName must not exceed 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Min password length is 6");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email form");
        }
    }
}
